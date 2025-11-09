using PortfolioAdmin.Api.Data;
using PortfolioAdmin.Api.Models;
using BCrypt.Net;

namespace PortfolioAdmin.Api.Services;

public interface IAuthService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User> CreateUserAsync(string email, string password, string role = "Admin");
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        // Update last login
        await _userRepository.UpdateLastLoginAsync(user.Id);
        user.LastLoginAt = DateTime.UtcNow;

        return user;
    }

    public async Task<User> CreateUserAsync(string email, string password, string role = "Admin")
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = HashPassword(password),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        return await _userRepository.CreateAsync(user);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            return false;
        }
    }
}