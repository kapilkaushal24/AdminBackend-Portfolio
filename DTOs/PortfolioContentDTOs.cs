namespace PortfolioAdmin.Api.DTOs
{
    // Hero Section DTOs
    public record HeroSectionDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string HeroContent { get; init; } = string.Empty;
        public string? ResumeUrl { get; init; }
        public string ContactEmail { get; init; } = string.Empty;
        public string? ProfileImageUrl { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record UpdateHeroSectionDto
    {
        public string Name { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string HeroContent { get; init; } = string.Empty;
        public string? ResumeUrl { get; init; }
        public string ContactEmail { get; init; } = string.Empty;
        public string? ProfileImageUrl { get; init; }
    }

    // About Section DTOs
    public record AboutSectionDto
    {
        public int Id { get; init; }
        public string AboutText { get; init; } = string.Empty;
        public string? ProfileImageUrl { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record UpdateAboutSectionDto
    {
        public string AboutText { get; init; } = string.Empty;
        public string? ProfileImageUrl { get; init; }
    }

    // Technology DTOs
    public record TechnologyDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string? IconUrl { get; init; }
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record CreateTechnologyDto
    {
        public string Name { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string? IconUrl { get; init; }
        public int SortOrder { get; init; }
    }

    public record UpdateTechnologyDto
    {
        public string Name { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string? IconUrl { get; init; }
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
    }

    // Experience DTOs
    public record ExperienceDto
    {
        public int Id { get; init; }
        public string Year { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string Company { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public List<string> Technologies { get; init; } = new();
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record CreateExperienceDto
    {
        public string Year { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string Company { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public List<string> Technologies { get; init; } = new();
        public int SortOrder { get; init; }
    }

    public record UpdateExperienceDto
    {
        public string Year { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string Company { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public List<string> Technologies { get; init; } = new();
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
    }

    // Portfolio Project DTOs
    public record PortfolioProjectDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public List<string> Technologies { get; init; } = new();
        public string? LiveUrl { get; init; }
        public string? GithubUrl { get; init; }
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record CreatePortfolioProjectDto
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public List<string> Technologies { get; init; } = new();
        public string? LiveUrl { get; init; }
        public string? GithubUrl { get; init; }
        public int SortOrder { get; init; }
    }

    public record UpdatePortfolioProjectDto
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public List<string> Technologies { get; init; } = new();
        public string? LiveUrl { get; init; }
        public string? GithubUrl { get; init; }
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
    }

    // Contact Info DTOs
    public record ContactInfoDto
    {
        public int Id { get; init; }
        public string Address { get; init; } = string.Empty;
        public string PhoneNo { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? LinkedInUrl { get; init; }
        public string? GithubUrl { get; init; }
        public string? InstagramUrl { get; init; }
        public string? TwitterUrl { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record UpdateContactInfoDto
    {
        public string Address { get; init; } = string.Empty;
        public string PhoneNo { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? LinkedInUrl { get; init; }
        public string? GithubUrl { get; init; }
        public string? InstagramUrl { get; init; }
        public string? TwitterUrl { get; init; }
    }

    // Generative AI Section DTOs
    public record GenerativeAISectionDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string SectionType { get; init; } = string.Empty;
        public string? Status { get; init; }
        public List<string>? Technologies { get; init; }
        public List<string>? ProgressItems { get; init; }
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }

    public record CreateGenerativeAISectionDto
    {
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string SectionType { get; init; } = string.Empty;
        public string? Status { get; init; }
        public List<string>? Technologies { get; init; }
        public List<string>? ProgressItems { get; init; }
        public int SortOrder { get; init; }
    }

    public record UpdateGenerativeAISectionDto
    {
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string SectionType { get; init; } = string.Empty;
        public string? Status { get; init; }
        public List<string>? Technologies { get; init; }
        public List<string>? ProgressItems { get; init; }
        public int SortOrder { get; init; }
        public bool IsActive { get; init; }
    }
}