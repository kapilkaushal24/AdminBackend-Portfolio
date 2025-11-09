using Dapper;
using PortfolioAdmin.Api.Models;

namespace PortfolioAdmin.Api.Data;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();
        const string sql = "SELECT * FROM Users WHERE Email = @Email AND IsActive = 1";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        const string sql = "SELECT * FROM Users WHERE Id = @Id AND IsActive = 1";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection = _context.CreateConnection();
        const string sql = @"
            INSERT INTO Users (Email, PasswordHash, Role, IsActive, CreatedAt)
            VALUES (@Email, @PasswordHash, @Role, @IsActive, @CreatedAt);
            SELECT last_insert_rowid();";

        var id = await connection.QuerySingleAsync<int>(sql, user);
        user.Id = id;
        return user;
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        using var connection = _context.CreateConnection();
        const string sql = "UPDATE Users SET LastLoginAt = @LastLoginAt WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = userId, LastLoginAt = DateTime.UtcNow });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        const string sql = "SELECT * FROM Users WHERE IsActive = 1 ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<User>(sql);
    }
}