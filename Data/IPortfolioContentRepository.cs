using PortfolioAdmin.Api.Models;

namespace PortfolioAdmin.Api.Data
{
    public interface IPortfolioContentRepository
    {
        // Hero Section
        Task<HeroSection?> GetHeroSectionAsync();
        Task<HeroSection> UpdateHeroSectionAsync(HeroSection heroSection);

        // About Section
        Task<AboutSection?> GetAboutSectionAsync();
        Task<AboutSection> UpdateAboutSectionAsync(AboutSection aboutSection);

        // Technologies
        Task<IEnumerable<Technology>> GetTechnologiesAsync();
        Task<Technology?> GetTechnologyByIdAsync(int id);
        Task<Technology> CreateTechnologyAsync(Technology technology);
        Task<Technology> UpdateTechnologyAsync(Technology technology);
        Task<bool> DeleteTechnologyAsync(int id);

        // Portfolio Experience
        Task<IEnumerable<PortfolioExperience>> GetExperiencesAsync();
        Task<PortfolioExperience?> GetExperienceByIdAsync(int id);
        Task<PortfolioExperience> CreateExperienceAsync(PortfolioExperience experience);
        Task<PortfolioExperience> UpdateExperienceAsync(PortfolioExperience experience);
        Task<bool> DeleteExperienceAsync(int id);

        // Portfolio Projects
        Task<IEnumerable<PortfolioProject>> GetProjectsAsync();
        Task<PortfolioProject?> GetProjectByIdAsync(int id);
        Task<PortfolioProject> CreateProjectAsync(PortfolioProject project);
        Task<PortfolioProject> UpdateProjectAsync(PortfolioProject project);
        Task<bool> DeleteProjectAsync(int id);

        // Contact Info
        Task<ContactInfo?> GetContactInfoAsync();
        Task<ContactInfo> UpdateContactInfoAsync(ContactInfo contactInfo);

        // Generative AI Sections
        Task<IEnumerable<GenerativeAISection>> GetGenerativeAISectionsAsync();
        Task<GenerativeAISection?> GetGenerativeAISectionByIdAsync(int id);
        Task<GenerativeAISection> CreateGenerativeAISectionAsync(GenerativeAISection section);
        Task<GenerativeAISection> UpdateGenerativeAISectionAsync(GenerativeAISection section);
        Task<bool> DeleteGenerativeAISectionAsync(int id);
    }
}