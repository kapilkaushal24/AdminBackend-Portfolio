using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using PortfolioAdmin.Api.Data;

namespace PortfolioAdmin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires JWT authentication
public class DatabaseController : ControllerBase
{
    private readonly DatabaseContext _dbContext;
    private readonly IConfiguration _configuration;

    public DatabaseController(DatabaseContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    /// <summary>
    /// Download the SQLite database file
    /// </summary>
    [HttpGet("download")]
    public IActionResult DownloadDatabase()
    {
        try
        {
            // Get the database file path
            var connectionString = _dbContext.CreateConnection().ConnectionString;
            var builder = new SqliteConnectionStringBuilder(connectionString);
            var dbPath = builder.DataSource;

            if (!System.IO.File.Exists(dbPath))
            {
                return NotFound(new { message = "Database file not found" });
            }

            // Read the database file
            var fileBytes = System.IO.File.ReadAllBytes(dbPath);
            var fileName = $"portfolio-backup-{DateTime.UtcNow:yyyyMMdd-HHmmss}.db";

            return File(fileBytes, "application/x-sqlite3", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error downloading database", error = ex.Message });
        }
    }

    /// <summary>
    /// Get database information (tables, row counts, etc.)
    /// </summary>
    [HttpGet("info")]
    public async Task<IActionResult> GetDatabaseInfo()
    {
        try
        {
            using var connection = (SqliteConnection)_dbContext.CreateConnection();
            await connection.OpenAsync();

            var tables = new List<object>();

            // Get all tables
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%' ORDER BY name";
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var tableName = reader.GetString(0);

                    // Get row count for this table
                    using var countCmd = connection.CreateCommand();
                    countCmd.CommandText = $"SELECT COUNT(*) FROM {tableName}";
                    var rowCount = Convert.ToInt64(await countCmd.ExecuteScalarAsync());

                    tables.Add(new
                    {
                        name = tableName,
                        rowCount = rowCount
                    });
                }
            }

            // Get database file info
            var connectionString = connection.ConnectionString;
            var builder = new SqliteConnectionStringBuilder(connectionString);
            var dbPath = builder.DataSource;
            var fileInfo = new FileInfo(dbPath);

            return Ok(new
            {
                databasePath = dbPath,
                fileSize = fileInfo.Exists ? $"{fileInfo.Length / 1024.0:F2} KB" : "N/A",
                tables = tables,
                totalTables = tables.Count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error getting database info", error = ex.Message });
        }
    }

    /// <summary>
    /// Execute a SELECT query (read-only)
    /// </summary>
    [HttpPost("query")]
    public async Task<IActionResult> ExecuteQuery([FromBody] QueryRequest request)
    {
        try
        {
            // Only allow SELECT queries for safety
            if (!request.Query.Trim().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Only SELECT queries are allowed" });
            }

            using var connection = (SqliteConnection)_dbContext.CreateConnection();
            await connection.OpenAsync();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = request.Query;

            var results = new List<Dictionary<string, object?>>();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object?>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                results.Add(row);
            }

            return Ok(new
            {
                rowCount = results.Count,
                data = results
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error executing query", error = ex.Message });
        }
    }
}

public record QueryRequest(string Query);
