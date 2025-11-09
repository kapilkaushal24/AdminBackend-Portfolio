using Dapper;
using PortfolioAdmin.Api.Models;
using System.Text.Json;

namespace PortfolioAdmin.Api.Data;

public class ProjectRepository : IProjectRepository
{
    private readonly DatabaseContext _context;

    public ProjectRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        const string sql = "SELECT * FROM Projects ORDER BY SortOrder, CreatedAt DESC";
        return await connection.QueryAsync<Project>(sql);
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        const string sql = "SELECT * FROM Projects WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Project>(sql, new { Id = id });
    }

    public async Task<Project> CreateAsync(Project project)
    {
        using var connection = _context.CreateConnection();
        const string sql = @"
            INSERT INTO Projects (Title, Description, LongDescription, ImageUrl, GithubUrl, LiveUrl, 
                                Technologies, IsActive, SortOrder, CreatedAt, UpdatedAt)
            VALUES (@Title, @Description, @LongDescription, @ImageUrl, @GithubUrl, @LiveUrl, 
                   @Technologies, @IsActive, @SortOrder, @CreatedAt, @UpdatedAt);
            SELECT last_insert_rowid();";

        var id = await connection.QuerySingleAsync<int>(sql, project);
        project.Id = id;
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        using var connection = _context.CreateConnection();
        const string sql = @"
            UPDATE Projects 
            SET Title = @Title, Description = @Description, LongDescription = @LongDescription,
                ImageUrl = @ImageUrl, GithubUrl = @GithubUrl, LiveUrl = @LiveUrl,
                Technologies = @Technologies, IsActive = @IsActive, SortOrder = @SortOrder,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id";

        await connection.ExecuteAsync(sql, project);
        return project;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        const string sql = "DELETE FROM Projects WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Project>> GetActiveAsync()
    {
        using var connection = _context.CreateConnection();
        const string sql = "SELECT * FROM Projects WHERE IsActive = 1 ORDER BY SortOrder, CreatedAt DESC";
        return await connection.QueryAsync<Project>(sql);
    }
}