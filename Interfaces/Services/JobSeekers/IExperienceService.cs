using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers
{
    public interface IExperienceService
    {
        Task<List<Experience>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<Experience?> GetByIdAsync(int id);

        Task<Experience> AddAsync(
            int jobSeekerId,
            AddExperienceDto dto);

        Task<Experience?> UpdateAsync(
            int jobSeekerId,
            int experienceId,
            AddExperienceDto dto);

        Task<bool> DeleteAsync(
            int jobSeekerId,
            int experienceId);
    }
}