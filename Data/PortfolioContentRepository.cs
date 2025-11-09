using Dapper;
using PortfolioAdmin.Api.Models;
using System.Text.Json;

namespace PortfolioAdmin.Api.Data
{
    public class PortfolioContentRepository : IPortfolioContentRepository
    {
        private readonly DatabaseContext _context;

        public PortfolioContentRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Hero Section
        public async Task<HeroSection?> GetHeroSectionAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM HeroSection WHERE Id = 1";
            return await connection.QueryFirstOrDefaultAsync<HeroSection>(sql);
        }

        public async Task<HeroSection> UpdateHeroSectionAsync(HeroSection heroSection)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT OR REPLACE INTO HeroSection (Id, Name, Role, HeroContent, ResumeUrl, ContactEmail, ProfileImageUrl, UpdatedAt)
                VALUES (1, @Name, @Role, @HeroContent, @ResumeUrl, @ContactEmail, @ProfileImageUrl, @UpdatedAt)";
            
            heroSection.UpdatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, heroSection);
            return heroSection;
        }

        // About Section
        public async Task<AboutSection?> GetAboutSectionAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM AboutSection WHERE Id = 1";
            return await connection.QueryFirstOrDefaultAsync<AboutSection>(sql);
        }

        public async Task<AboutSection> UpdateAboutSectionAsync(AboutSection aboutSection)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT OR REPLACE INTO AboutSection (Id, AboutText, ProfileImageUrl, UpdatedAt)
                VALUES (1, @AboutText, @ProfileImageUrl, @UpdatedAt)";
            
            aboutSection.UpdatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, aboutSection);
            return aboutSection;
        }

        // Technologies
        public async Task<IEnumerable<Technology>> GetTechnologiesAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Technologies WHERE IsActive = 1 ORDER BY SortOrder, Name";
            return await connection.QueryAsync<Technology>(sql);
        }

        public async Task<Technology?> GetTechnologyByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Technologies WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Technology>(sql, new { Id = id });
        }

        public async Task<Technology> CreateTechnologyAsync(Technology technology)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO Technologies (Name, Category, IconUrl, SortOrder, IsActive, CreatedAt)
                VALUES (@Name, @Category, @IconUrl, @SortOrder, @IsActive, @CreatedAt)
                RETURNING *";
            
            technology.CreatedAt = DateTime.UtcNow;
            return await connection.QuerySingleAsync<Technology>(sql, technology);
        }

        public async Task<Technology> UpdateTechnologyAsync(Technology technology)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE Technologies 
                SET Name = @Name, Category = @Category, IconUrl = @IconUrl, 
                    SortOrder = @SortOrder, IsActive = @IsActive
                WHERE Id = @Id";
            
            await connection.ExecuteAsync(sql, technology);
            return technology;
        }

        public async Task<bool> DeleteTechnologyAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM Technologies WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        // Portfolio Experience
        public async Task<IEnumerable<PortfolioExperience>> GetExperiencesAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM PortfolioExperience WHERE IsActive = 1 ORDER BY SortOrder";
            return await connection.QueryAsync<PortfolioExperience>(sql);
        }

        public async Task<PortfolioExperience?> GetExperienceByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM PortfolioExperience WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<PortfolioExperience>(sql, new { Id = id });
        }

        public async Task<PortfolioExperience> CreateExperienceAsync(PortfolioExperience experience)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO PortfolioExperience (Year, Role, Company, Description, Technologies, SortOrder, IsActive, CreatedAt, UpdatedAt)
                VALUES (@Year, @Role, @Company, @Description, @Technologies, @SortOrder, @IsActive, @CreatedAt, @UpdatedAt)
                RETURNING *";
            
            experience.CreatedAt = DateTime.UtcNow;
            experience.UpdatedAt = DateTime.UtcNow;
            return await connection.QuerySingleAsync<PortfolioExperience>(sql, experience);
        }

        public async Task<PortfolioExperience> UpdateExperienceAsync(PortfolioExperience experience)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE PortfolioExperience 
                SET Year = @Year, Role = @Role, Company = @Company, Description = @Description,
                    Technologies = @Technologies, SortOrder = @SortOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            experience.UpdatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, experience);
            return experience;
        }

        public async Task<bool> DeleteExperienceAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM PortfolioExperience WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        // Portfolio Projects
        public async Task<IEnumerable<PortfolioProject>> GetProjectsAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM PortfolioProjects WHERE IsActive = 1 ORDER BY SortOrder";
            return await connection.QueryAsync<PortfolioProject>(sql);
        }

        public async Task<PortfolioProject?> GetProjectByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM PortfolioProjects WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<PortfolioProject>(sql, new { Id = id });
        }

        public async Task<PortfolioProject> CreateProjectAsync(PortfolioProject project)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO PortfolioProjects (Title, Description, ImageUrl, Technologies, LiveUrl, GithubUrl, SortOrder, IsActive, CreatedAt, UpdatedAt)
                VALUES (@Title, @Description, @ImageUrl, @Technologies, @LiveUrl, @GithubUrl, @SortOrder, @IsActive, @CreatedAt, @UpdatedAt)
                RETURNING *";
            
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;
            return await connection.QuerySingleAsync<PortfolioProject>(sql, project);
        }

        public async Task<PortfolioProject> UpdateProjectAsync(PortfolioProject project)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE PortfolioProjects 
                SET Title = @Title, Description = @Description, ImageUrl = @ImageUrl,
                    Technologies = @Technologies, LiveUrl = @LiveUrl, GithubUrl = @GithubUrl,
                    SortOrder = @SortOrder, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            project.UpdatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, project);
            return project;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM PortfolioProjects WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        // Contact Info
        public async Task<ContactInfo?> GetContactInfoAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM ContactInfo WHERE Id = 1";
            return await connection.QueryFirstOrDefaultAsync<ContactInfo>(sql);
        }

        public async Task<ContactInfo> UpdateContactInfoAsync(ContactInfo contactInfo)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT OR REPLACE INTO ContactInfo (Id, Address, PhoneNo, Email, LinkedInUrl, GithubUrl, InstagramUrl, TwitterUrl, UpdatedAt)
                VALUES (1, @Address, @PhoneNo, @Email, @LinkedInUrl, @GithubUrl, @InstagramUrl, @TwitterUrl, @UpdatedAt)";
            
            contactInfo.UpdatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, contactInfo);
            return contactInfo;
        }

        // Generative AI Sections
        public async Task<IEnumerable<GenerativeAISection>> GetGenerativeAISectionsAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM GenerativeAISection WHERE IsActive = 1 ORDER BY SortOrder";
            return await connection.QueryAsync<GenerativeAISection>(sql);
        }

        public async Task<GenerativeAISection?> GetGenerativeAISectionByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM GenerativeAISection WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<GenerativeAISection>(sql, new { Id = id });
        }

        public async Task<GenerativeAISection> CreateGenerativeAISectionAsync(GenerativeAISection section)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                INSERT INTO GenerativeAISection (Title, Content, SectionType, Status, Technologies, ProgressItems, SortOrder, IsActive, CreatedAt, UpdatedAt)
                VALUES (@Title, @Content, @SectionType, @Status, @Technologies, @ProgressItems, @SortOrder, @IsActive, @CreatedAt, @UpdatedAt)
                RETURNING *";
            
            section.CreatedAt = DateTime.UtcNow;
            section.UpdatedAt = DateTime.UtcNow;
            return await connection.QuerySingleAsync<GenerativeAISection>(sql, section);
        }

        public async Task<GenerativeAISection> UpdateGenerativeAISectionAsync(GenerativeAISection section)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                UPDATE GenerativeAISection 
                SET Title = @Title, Content = @Content, SectionType = @SectionType, Status = @Status,
                    Technologies = @Technologies, ProgressItems = @ProgressItems, SortOrder = @SortOrder,
                    IsActive = @IsActive, UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            section.UpdatedAt = DateTime.UtcNow;
            await connection.ExecuteAsync(sql, section);
            return section;
        }

        public async Task<bool> DeleteGenerativeAISectionAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM GenerativeAISection WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}