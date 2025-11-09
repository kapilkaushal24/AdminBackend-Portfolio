using PortfolioAdmin.Api.Models;

namespace PortfolioAdmin.Api.Data;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task UpdateLastLoginAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync();
}