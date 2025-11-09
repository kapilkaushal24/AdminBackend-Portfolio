namespace PortfolioAdmin.Api.Models;

public class PersonalInfo
{
    public int Id { get; set; } = 1; // Only one record
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? ResumeUrl { get; set; }
    public string Tagline { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}