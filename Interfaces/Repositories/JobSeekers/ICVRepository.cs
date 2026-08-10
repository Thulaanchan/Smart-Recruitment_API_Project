using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers
{
    public interface ICVRepository
    {
        Task<CV?> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<CV> CreateAsync(CV cv);

        Task<CV> UpdateAsync(CV cv);

        Task DeleteAsync(CV cv);
    }
}