using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers
{
    public interface IExperienceRepository
    {
        Task<List<Experience>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<Experience?> GetByIdAsync(int id);

        Task<Experience> CreateAsync(Experience experience);

        Task UpdateAsync(Experience experience);

        Task DeleteAsync(Experience experience);
    }
}