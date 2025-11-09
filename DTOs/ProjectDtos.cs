using System.ComponentModel.DataAnnotations;

namespace PortfolioAdmin.Api.DTOs;

public class ProjectDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public string? LongDescription { get; set; }
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }

    [Required]
    public List<string> Technologies { get; set; } = new();

    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 0;
}

public class CreateProjectDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public string? LongDescription { get; set; }
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }

    [Required]
    public List<string> Technologies { get; set; } = new();

    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 0;
}

public class UpdateProjectDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public string? LongDescription { get; set; }
    public string? ImageUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? LiveUrl { get; set; }
    public List<string>? Technologies { get; set; }
    public bool? IsActive { get; set; }
    public int? SortOrder { get; set; }
}