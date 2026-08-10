using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers
{
    public interface IEducationService
    {
        Task<List<Education>> GetByJobSeekerIdAsync(
            int jobSeekerId);

        Task<Education?> GetByIdAsync(int id);

        Task<Education> AddAsync(
            int jobSeekerId,
            AddEducationDto dto);

        Task<Education?> UpdateAsync(
            int jobSeekerId,
            int educationId,
            AddEducationDto dto);

        Task<bool> DeleteAsync(
            int jobSeekerId,
            int educationId);
    }
}