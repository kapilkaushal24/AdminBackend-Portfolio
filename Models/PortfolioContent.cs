using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioAdmin.Api.Models
{
    public class HeroSection
    {
        public int Id { get; set; }
        
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Role { get; set; } = string.Empty;
        
        [Column(TypeName = "TEXT")] // SQLite: TEXT (unlimited), SQL Server: nvarchar(max)
        public string HeroContent { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? ResumeUrl { get; set; }
        
        [MaxLength(200)]
        public string ContactEmail { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class AboutSection
    {
        public int Id { get; set; }
        
        [Column(TypeName = "TEXT")] // SQLite: TEXT (unlimited), SQL Server: nvarchar(max)
        public string AboutText { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Technology
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // Backend, Frontend, AI/ML, etc.
        
        [MaxLength(500)]
        public string? IconUrl { get; set; }
        
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PortfolioExperience
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Year { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Role { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Company { get; set; } = string.Empty;
        
        [Column(TypeName = "TEXT")] // SQLite: TEXT (unlimited), SQL Server: nvarchar(max)
        public string Description { get; set; } = string.Empty;
        
        [Column(TypeName = "TEXT")] // JSON array of technology names
        public string Technologies { get; set; } = string.Empty;
        
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PortfolioProject
    {
        public int Id { get; set; }
        
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;
        
        [Column(TypeName = "TEXT")] // SQLite: TEXT (unlimited), SQL Server: nvarchar(max)
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? ImageUrl { get; set; }
        
        [Column(TypeName = "TEXT")] // JSON array of technology names
        public string Technologies { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? LiveUrl { get; set; }
        
        [MaxLength(500)]
        public string? GithubUrl { get; set; }
        
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ContactInfo
    {
        public int Id { get; set; }
        
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string PhoneNo { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? LinkedInUrl { get; set; }
        
        [MaxLength(500)]
        public string? GithubUrl { get; set; }
        
        [MaxLength(500)]
        public string? InstagramUrl { get; set; }
        
        [MaxLength(500)]
        public string? TwitterUrl { get; set; }
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class GenerativeAISection
    {
        public int Id { get; set; }
        
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;
        
        [Column(TypeName = "TEXT")] // SQLite: TEXT (unlimited), SQL Server: nvarchar(max)
        public string Content { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string SectionType { get; set; } = string.Empty; // Course, Roadmap, Vision
        
        [MaxLength(50)]
        public string? Status { get; set; } // In Progress, Completed, Planned
        
        [Column(TypeName = "TEXT")] // JSON array
        public string? Technologies { get; set; }
        
        [Column(TypeName = "TEXT")] // JSON array of progress items
        public string? ProgressItems { get; set; }
        
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}