using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers
{
    public interface IEducationRepository
    {
        Task<List<Education>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<Education?> GetByIdAsync(int id);

        Task<Education> CreateAsync(Education education);

        Task UpdateAsync(Education education);

        Task DeleteAsync(Education education);
    }
}