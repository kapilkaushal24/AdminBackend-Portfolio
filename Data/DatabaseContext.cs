using Microsoft.Data.Sqlite;
using System.Data;

namespace PortfolioAdmin.Api.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=portfolio.db";
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public async Task InitializeAsync()
    {
        using var connection = (SqliteConnection)CreateConnection();
        await connection.OpenAsync();

        // Create Users table
        var createUsersTable = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Email TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                Role TEXT NOT NULL DEFAULT 'Admin',
                IsActive INTEGER NOT NULL DEFAULT 1,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                LastLoginAt TEXT
            )";

        // Create Projects table
        var createProjectsTable = @"
            CREATE TABLE IF NOT EXISTS Projects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT NOT NULL,
                LongDescription TEXT,
                ImageUrl TEXT,
                GithubUrl TEXT,
                LiveUrl TEXT,
                Technologies TEXT NOT NULL,
                IsActive INTEGER NOT NULL DEFAULT 1,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create Experience table
        var createExperienceTable = @"
            CREATE TABLE IF NOT EXISTS Experience (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                JobTitle TEXT NOT NULL,
                Company TEXT NOT NULL,
                CompanyUrl TEXT,
                Location TEXT NOT NULL,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                IsCurrent INTEGER NOT NULL DEFAULT 0,
                Description TEXT NOT NULL,
                Skills TEXT NOT NULL,
                IsActive INTEGER NOT NULL DEFAULT 1,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create Skills table
        var createSkillsTable = @"
            CREATE TABLE IF NOT EXISTS Skills (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT NOT NULL,
                ProficiencyLevel INTEGER NOT NULL DEFAULT 1,
                IconUrl TEXT,
                IsActive INTEGER NOT NULL DEFAULT 1,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create PersonalInfo table
        var createPersonalInfoTable = @"
            CREATE TABLE IF NOT EXISTS PersonalInfo (
                Id INTEGER PRIMARY KEY DEFAULT 1,
                FullName TEXT NOT NULL,
                Email TEXT NOT NULL,
                Phone TEXT NOT NULL,
                LinkedInUrl TEXT,
                GitHubUrl TEXT,
                ResumeUrl TEXT,
                Tagline TEXT NOT NULL,
                About TEXT NOT NULL,
                ProfileImageUrl TEXT,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create HeroSection table
        var createHeroSectionTable = @"
            CREATE TABLE IF NOT EXISTS HeroSection (
                Id INTEGER PRIMARY KEY DEFAULT 1,
                Name TEXT NOT NULL,
                Role TEXT NOT NULL,
                HeroContent TEXT NOT NULL,
                ResumeUrl TEXT,
                ContactEmail TEXT NOT NULL,
                ProfileImageUrl TEXT,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create AboutSection table
        var createAboutSectionTable = @"
            CREATE TABLE IF NOT EXISTS AboutSection (
                Id INTEGER PRIMARY KEY DEFAULT 1,
                AboutText TEXT NOT NULL,
                ProfileImageUrl TEXT,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create Technologies table
        var createTechnologiesTable = @"
            CREATE TABLE IF NOT EXISTS Technologies (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT NOT NULL,
                IconUrl TEXT,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create PortfolioExperience table (renamed to avoid conflict)
        var createPortfolioExperienceTable = @"
            CREATE TABLE IF NOT EXISTS PortfolioExperience (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Year TEXT NOT NULL,
                Role TEXT NOT NULL,
                Company TEXT NOT NULL,
                Description TEXT NOT NULL,
                Technologies TEXT NOT NULL,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create PortfolioProjects table (renamed to avoid conflict)
        var createPortfolioProjectsTable = @"
            CREATE TABLE IF NOT EXISTS PortfolioProjects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT NOT NULL,
                ImageUrl TEXT,
                Technologies TEXT NOT NULL,
                LiveUrl TEXT,
                GithubUrl TEXT,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create ContactInfo table
        var createContactInfoTable = @"
            CREATE TABLE IF NOT EXISTS ContactInfo (
                Id INTEGER PRIMARY KEY DEFAULT 1,
                Address TEXT NOT NULL,
                PhoneNo TEXT NOT NULL,
                Email TEXT NOT NULL,
                LinkedInUrl TEXT,
                GithubUrl TEXT,
                InstagramUrl TEXT,
                TwitterUrl TEXT,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        // Create GenerativeAISection table
        var createGenerativeAISectionTable = @"
            CREATE TABLE IF NOT EXISTS GenerativeAISection (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT NOT NULL,
                SectionType TEXT NOT NULL,
                Status TEXT,
                Technologies TEXT,
                ProgressItems TEXT,
                SortOrder INTEGER NOT NULL DEFAULT 0,
                IsActive INTEGER NOT NULL DEFAULT 1,
                CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                UpdatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
            )";

        var tables = new[] { 
            createUsersTable, 
            createProjectsTable, 
            createExperienceTable, 
            createSkillsTable, 
            createPersonalInfoTable,
            createHeroSectionTable,
            createAboutSectionTable,
            createTechnologiesTable,
            createPortfolioExperienceTable,
            createPortfolioProjectsTable,
            createContactInfoTable,
            createGenerativeAISectionTable
        };
        
        foreach (var tableQuery in tables)
        {
            using var command = connection.CreateCommand();
            command.CommandText = tableQuery;
            await command.ExecuteNonQueryAsync();
        }

        // Migration: Add ProfileImageUrl column to HeroSection if it doesn't exist
        try
        {
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT ProfileImageUrl FROM HeroSection LIMIT 1";
            await checkCommand.ExecuteScalarAsync();
        }
        catch
        {
            // Column doesn't exist, add it
            using var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE HeroSection ADD COLUMN ProfileImageUrl TEXT";
            await alterCommand.ExecuteNonQueryAsync();
        }

        // Migration: Add ProfileImageUrl column to AboutSection if it doesn't exist
        try
        {
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT ProfileImageUrl FROM AboutSection LIMIT 1";
            await checkCommand.ExecuteScalarAsync();
        }
        catch
        {
            // Column doesn't exist, add it
            using var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE AboutSection ADD COLUMN ProfileImageUrl TEXT";
            await alterCommand.ExecuteNonQueryAsync();
        }
    }
}