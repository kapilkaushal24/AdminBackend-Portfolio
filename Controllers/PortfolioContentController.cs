using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioAdmin.Api.Data;
using PortfolioAdmin.Api.DTOs;
using PortfolioAdmin.Api.Models;
using System.Text.Json;

namespace PortfolioAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioContentController : ControllerBase
    {
        private readonly IPortfolioContentRepository _repository;

        public PortfolioContentController(IPortfolioContentRepository repository)
        {
            _repository = repository;
        }

        // Hero Section Endpoints
        [HttpGet("hero")]
        public async Task<ActionResult<HeroSectionDto>> GetHeroSection()
        {
            var heroSection = await _repository.GetHeroSectionAsync();
            if (heroSection == null)
            {
                return NotFound();
            }

            var dto = new HeroSectionDto
            {
                Id = heroSection.Id,
                Name = heroSection.Name,
                Role = heroSection.Role,
                HeroContent = heroSection.HeroContent,
                ResumeUrl = heroSection.ResumeUrl,
                ContactEmail = heroSection.ContactEmail,
                ProfileImageUrl = heroSection.ProfileImageUrl,
                UpdatedAt = heroSection.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPut("hero")]
        [Authorize]
        public async Task<ActionResult<HeroSectionDto>> UpdateHeroSection([FromBody] UpdateHeroSectionDto request)
        {
            var heroSection = new HeroSection
            {
                Id = 1,
                Name = request.Name,
                Role = request.Role,
                HeroContent = request.HeroContent,
                ResumeUrl = request.ResumeUrl,
                ContactEmail = request.ContactEmail,
                ProfileImageUrl = request.ProfileImageUrl
            };

            var updated = await _repository.UpdateHeroSectionAsync(heroSection);
            
            var dto = new HeroSectionDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Role = updated.Role,
                HeroContent = updated.HeroContent,
                ResumeUrl = updated.ResumeUrl,
                ContactEmail = updated.ContactEmail,
                ProfileImageUrl = updated.ProfileImageUrl,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(dto);
        }

        // About Section Endpoints
        [HttpGet("about")]
        public async Task<ActionResult<AboutSectionDto>> GetAboutSection()
        {
            var aboutSection = await _repository.GetAboutSectionAsync();
            if (aboutSection == null)
            {
                return NotFound();
            }

            var dto = new AboutSectionDto
            {
                Id = aboutSection.Id,
                AboutText = aboutSection.AboutText,
                ProfileImageUrl = aboutSection.ProfileImageUrl,
                UpdatedAt = aboutSection.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPut("about")]
        [Authorize]
        public async Task<ActionResult<AboutSectionDto>> UpdateAboutSection([FromBody] UpdateAboutSectionDto request)
        {
            var aboutSection = new AboutSection
            {
                Id = 1,
                AboutText = request.AboutText,
                ProfileImageUrl = request.ProfileImageUrl
            };

            var updated = await _repository.UpdateAboutSectionAsync(aboutSection);
            
            var dto = new AboutSectionDto
            {
                Id = updated.Id,
                AboutText = updated.AboutText,
                ProfileImageUrl = updated.ProfileImageUrl,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(dto);
        }

        // Technologies Endpoints
        [HttpGet("technologies")]
        public async Task<ActionResult<IEnumerable<TechnologyDto>>> GetTechnologies()
        {
            var technologies = await _repository.GetTechnologiesAsync();
            var dtos = technologies.Select(t => new TechnologyDto
            {
                Id = t.Id,
                Name = t.Name,
                Category = t.Category,
                IconUrl = t.IconUrl,
                SortOrder = t.SortOrder,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            });

            return Ok(dtos);
        }

        [HttpGet("technologies/{id}")]
        public async Task<ActionResult<TechnologyDto>> GetTechnology(int id)
        {
            var technology = await _repository.GetTechnologyByIdAsync(id);
            if (technology == null)
            {
                return NotFound();
            }

            var dto = new TechnologyDto
            {
                Id = technology.Id,
                Name = technology.Name,
                Category = technology.Category,
                IconUrl = technology.IconUrl,
                SortOrder = technology.SortOrder,
                IsActive = technology.IsActive,
                CreatedAt = technology.CreatedAt
            };

            return Ok(dto);
        }

        [HttpPost("technologies")]
        [Authorize]
        public async Task<ActionResult<TechnologyDto>> CreateTechnology([FromBody] CreateTechnologyDto request)
        {
            var technology = new Technology
            {
                Name = request.Name,
                Category = request.Category,
                IconUrl = request.IconUrl,
                SortOrder = request.SortOrder,
                IsActive = true
            };

            var created = await _repository.CreateTechnologyAsync(technology);
            
            var dto = new TechnologyDto
            {
                Id = created.Id,
                Name = created.Name,
                Category = created.Category,
                IconUrl = created.IconUrl,
                SortOrder = created.SortOrder,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetTechnology), new { id = created.Id }, dto);
        }

        [HttpPut("technologies/{id}")]
        [Authorize]
        public async Task<ActionResult<TechnologyDto>> UpdateTechnology(int id, [FromBody] UpdateTechnologyDto request)
        {
            var technology = new Technology
            {
                Id = id,
                Name = request.Name,
                Category = request.Category,
                IconUrl = request.IconUrl,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            var updated = await _repository.UpdateTechnologyAsync(technology);
            
            var dto = new TechnologyDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Category = updated.Category,
                IconUrl = updated.IconUrl,
                SortOrder = updated.SortOrder,
                IsActive = updated.IsActive,
                CreatedAt = updated.CreatedAt
            };

            return Ok(dto);
        }

        [HttpDelete("technologies/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteTechnology(int id)
        {
            var deleted = await _repository.DeleteTechnologyAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Experience Endpoints
        [HttpGet("experiences")]
        public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences()
        {
            var experiences = await _repository.GetExperiencesAsync();
            var dtos = experiences.Select(exp => new ExperienceDto
            {
                Id = exp.Id,
                Year = exp.Year,
                Role = exp.Role,
                Company = exp.Company,
                Description = exp.Description,
                Technologies = string.IsNullOrEmpty(exp.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(exp.Technologies) ?? new List<string>(),
                SortOrder = exp.SortOrder,
                IsActive = exp.IsActive,
                CreatedAt = exp.CreatedAt,
                UpdatedAt = exp.UpdatedAt
            });

            return Ok(dtos);
        }

        [HttpGet("experiences/{id}")]
        public async Task<ActionResult<ExperienceDto>> GetExperience(int id)
        {
            var experience = await _repository.GetExperienceByIdAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            var dto = new ExperienceDto
            {
                Id = experience.Id,
                Year = experience.Year,
                Role = experience.Role,
                Company = experience.Company,
                Description = experience.Description,
                Technologies = string.IsNullOrEmpty(experience.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(experience.Technologies) ?? new List<string>(),
                SortOrder = experience.SortOrder,
                IsActive = experience.IsActive,
                CreatedAt = experience.CreatedAt,
                UpdatedAt = experience.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPost("experiences")]
        [Authorize]
        public async Task<ActionResult<ExperienceDto>> CreateExperience([FromBody] CreateExperienceDto request)
        {
            var experience = new PortfolioExperience
            {
                Year = request.Year,
                Role = request.Role,
                Company = request.Company,
                Description = request.Description,
                Technologies = JsonSerializer.Serialize(request.Technologies),
                SortOrder = request.SortOrder,
                IsActive = true
            };

            var created = await _repository.CreateExperienceAsync(experience);
            
            var dto = new ExperienceDto
            {
                Id = created.Id,
                Year = created.Year,
                Role = created.Role,
                Company = created.Company,
                Description = created.Description,
                Technologies = string.IsNullOrEmpty(created.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(created.Technologies) ?? new List<string>(),
                SortOrder = created.SortOrder,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return CreatedAtAction(nameof(GetExperience), new { id = created.Id }, dto);
        }

        [HttpPut("experiences/{id}")]
        [Authorize]
        public async Task<ActionResult<ExperienceDto>> UpdateExperience(int id, [FromBody] UpdateExperienceDto request)
        {
            var experience = new PortfolioExperience
            {
                Id = id,
                Year = request.Year,
                Role = request.Role,
                Company = request.Company,
                Description = request.Description,
                Technologies = JsonSerializer.Serialize(request.Technologies),
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            var updated = await _repository.UpdateExperienceAsync(experience);
            
            var dto = new ExperienceDto
            {
                Id = updated.Id,
                Year = updated.Year,
                Role = updated.Role,
                Company = updated.Company,
                Description = updated.Description,
                Technologies = string.IsNullOrEmpty(updated.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(updated.Technologies) ?? new List<string>(),
                SortOrder = updated.SortOrder,
                IsActive = updated.IsActive,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpDelete("experiences/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteExperience(int id)
        {
            var deleted = await _repository.DeleteExperienceAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Contact Info Endpoints
        [HttpGet("contact")]
        public async Task<ActionResult<ContactInfoDto>> GetContactInfo()
        {
            var contactInfo = await _repository.GetContactInfoAsync();
            if (contactInfo == null)
            {
                return NotFound();
            }

            var dto = new ContactInfoDto
            {
                Id = contactInfo.Id,
                Address = contactInfo.Address,
                PhoneNo = contactInfo.PhoneNo,
                Email = contactInfo.Email,
                LinkedInUrl = contactInfo.LinkedInUrl,
                GithubUrl = contactInfo.GithubUrl,
                InstagramUrl = contactInfo.InstagramUrl,
                TwitterUrl = contactInfo.TwitterUrl,
                UpdatedAt = contactInfo.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPut("contact")]
        [Authorize]
        public async Task<ActionResult<ContactInfoDto>> UpdateContactInfo([FromBody] UpdateContactInfoDto request)
        {
            var contactInfo = new ContactInfo
            {
                Id = 1,
                Address = request.Address,
                PhoneNo = request.PhoneNo,
                Email = request.Email,
                LinkedInUrl = request.LinkedInUrl,
                GithubUrl = request.GithubUrl,
                InstagramUrl = request.InstagramUrl,
                TwitterUrl = request.TwitterUrl
            };

            var updated = await _repository.UpdateContactInfoAsync(contactInfo);
            
            var dto = new ContactInfoDto
            {
                Id = updated.Id,
                Address = updated.Address,
                PhoneNo = updated.PhoneNo,
                Email = updated.Email,
                LinkedInUrl = updated.LinkedInUrl,
                GithubUrl = updated.GithubUrl,
                InstagramUrl = updated.InstagramUrl,
                TwitterUrl = updated.TwitterUrl,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(dto);
        }

        // Portfolio Projects Endpoints
        [HttpGet("projects")]
        public async Task<ActionResult<IEnumerable<PortfolioProjectDto>>> GetProjects()
        {
            var projects = await _repository.GetProjectsAsync();
            var dtos = projects.Select(proj => new PortfolioProjectDto
            {
                Id = proj.Id,
                Title = proj.Title,
                Description = proj.Description,
                ImageUrl = proj.ImageUrl,
                Technologies = string.IsNullOrEmpty(proj.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(proj.Technologies) ?? new List<string>(),
                LiveUrl = proj.LiveUrl,
                GithubUrl = proj.GithubUrl,
                SortOrder = proj.SortOrder,
                IsActive = proj.IsActive,
                CreatedAt = proj.CreatedAt,
                UpdatedAt = proj.UpdatedAt
            });

            return Ok(dtos);
        }

        [HttpGet("projects/{id}")]
        public async Task<ActionResult<PortfolioProjectDto>> GetProject(int id)
        {
            var project = await _repository.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var dto = new PortfolioProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                ImageUrl = project.ImageUrl,
                Technologies = string.IsNullOrEmpty(project.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(project.Technologies) ?? new List<string>(),
                LiveUrl = project.LiveUrl,
                GithubUrl = project.GithubUrl,
                SortOrder = project.SortOrder,
                IsActive = project.IsActive,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPost("projects")]
        [Authorize]
        public async Task<ActionResult<PortfolioProjectDto>> CreateProject([FromBody] CreatePortfolioProjectDto request)
        {
            var project = new PortfolioProject
            {
                Title = request.Title,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Technologies = JsonSerializer.Serialize(request.Technologies),
                LiveUrl = request.LiveUrl,
                GithubUrl = request.GithubUrl,
                SortOrder = request.SortOrder,
                IsActive = true
            };

            var created = await _repository.CreateProjectAsync(project);
            
            var dto = new PortfolioProjectDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                ImageUrl = created.ImageUrl,
                Technologies = string.IsNullOrEmpty(created.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(created.Technologies) ?? new List<string>(),
                LiveUrl = created.LiveUrl,
                GithubUrl = created.GithubUrl,
                SortOrder = created.SortOrder,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return CreatedAtAction(nameof(GetProject), new { id = created.Id }, dto);
        }

        [HttpPut("projects/{id}")]
        [Authorize]
        public async Task<ActionResult<PortfolioProjectDto>> UpdateProject(int id, [FromBody] UpdatePortfolioProjectDto request)
        {
            var project = new PortfolioProject
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Technologies = JsonSerializer.Serialize(request.Technologies),
                LiveUrl = request.LiveUrl,
                GithubUrl = request.GithubUrl,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            var updated = await _repository.UpdateProjectAsync(project);
            
            var dto = new PortfolioProjectDto
            {
                Id = updated.Id,
                Title = updated.Title,
                Description = updated.Description,
                ImageUrl = updated.ImageUrl,
                Technologies = string.IsNullOrEmpty(updated.Technologies) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(updated.Technologies) ?? new List<string>(),
                LiveUrl = updated.LiveUrl,
                GithubUrl = updated.GithubUrl,
                SortOrder = updated.SortOrder,
                IsActive = updated.IsActive,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpDelete("projects/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var deleted = await _repository.DeleteProjectAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Generative AI Sections Endpoints
        [HttpGet("genai-sections")]
        public async Task<ActionResult<IEnumerable<GenerativeAISectionDto>>> GetGenerativeAISections()
        {
            var sections = await _repository.GetGenerativeAISectionsAsync();
            var dtos = sections.Select(section => new GenerativeAISectionDto
            {
                Id = section.Id,
                Title = section.Title,
                Content = section.Content,
                SectionType = section.SectionType,
                Status = section.Status,
                Technologies = string.IsNullOrEmpty(section.Technologies) ? null : JsonSerializer.Deserialize<List<string>>(section.Technologies),
                ProgressItems = string.IsNullOrEmpty(section.ProgressItems) ? null : JsonSerializer.Deserialize<List<string>>(section.ProgressItems),
                SortOrder = section.SortOrder,
                IsActive = section.IsActive,
                CreatedAt = section.CreatedAt,
                UpdatedAt = section.UpdatedAt
            });

            return Ok(dtos);
        }

        [HttpGet("genai-sections/{id}")]
        public async Task<ActionResult<GenerativeAISectionDto>> GetGenerativeAISection(int id)
        {
            var section = await _repository.GetGenerativeAISectionByIdAsync(id);
            if (section == null)
            {
                return NotFound();
            }

            var dto = new GenerativeAISectionDto
            {
                Id = section.Id,
                Title = section.Title,
                Content = section.Content,
                SectionType = section.SectionType,
                Status = section.Status,
                Technologies = string.IsNullOrEmpty(section.Technologies) ? null : JsonSerializer.Deserialize<List<string>>(section.Technologies),
                ProgressItems = string.IsNullOrEmpty(section.ProgressItems) ? null : JsonSerializer.Deserialize<List<string>>(section.ProgressItems),
                SortOrder = section.SortOrder,
                IsActive = section.IsActive,
                CreatedAt = section.CreatedAt,
                UpdatedAt = section.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPost("genai-sections")]
        [Authorize]
        public async Task<ActionResult<GenerativeAISectionDto>> CreateGenerativeAISection([FromBody] CreateGenerativeAISectionDto request)
        {
            var section = new GenerativeAISection
            {
                Title = request.Title,
                Content = request.Content,
                SectionType = request.SectionType,
                Status = request.Status,
                Technologies = request.Technologies != null ? JsonSerializer.Serialize(request.Technologies) : null,
                ProgressItems = request.ProgressItems != null ? JsonSerializer.Serialize(request.ProgressItems) : null,
                SortOrder = request.SortOrder,
                IsActive = true
            };

            var created = await _repository.CreateGenerativeAISectionAsync(section);
            
            var dto = new GenerativeAISectionDto
            {
                Id = created.Id,
                Title = created.Title,
                Content = created.Content,
                SectionType = created.SectionType,
                Status = created.Status,
                Technologies = string.IsNullOrEmpty(created.Technologies) ? null : JsonSerializer.Deserialize<List<string>>(created.Technologies),
                ProgressItems = string.IsNullOrEmpty(created.ProgressItems) ? null : JsonSerializer.Deserialize<List<string>>(created.ProgressItems),
                SortOrder = created.SortOrder,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return CreatedAtAction(nameof(GetGenerativeAISection), new { id = created.Id }, dto);
        }

        [HttpPut("genai-sections/{id}")]
        [Authorize]
        public async Task<ActionResult<GenerativeAISectionDto>> UpdateGenerativeAISection(int id, [FromBody] UpdateGenerativeAISectionDto request)
        {
            var section = new GenerativeAISection
            {
                Id = id,
                Title = request.Title,
                Content = request.Content,
                SectionType = request.SectionType,
                Status = request.Status,
                Technologies = request.Technologies != null ? JsonSerializer.Serialize(request.Technologies) : null,
                ProgressItems = request.ProgressItems != null ? JsonSerializer.Serialize(request.ProgressItems) : null,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            var updated = await _repository.UpdateGenerativeAISectionAsync(section);
            
            var dto = new GenerativeAISectionDto
            {
                Id = updated.Id,
                Title = updated.Title,
                Content = updated.Content,
                SectionType = updated.SectionType,
                Status = updated.Status,
                Technologies = string.IsNullOrEmpty(updated.Technologies) ? null : JsonSerializer.Deserialize<List<string>>(updated.Technologies),
                ProgressItems = string.IsNullOrEmpty(updated.ProgressItems) ? null : JsonSerializer.Deserialize<List<string>>(updated.ProgressItems),
                SortOrder = updated.SortOrder,
                IsActive = updated.IsActive,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpDelete("genai-sections/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteGenerativeAISection(int id)
        {
            var deleted = await _repository.DeleteGenerativeAISectionAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}