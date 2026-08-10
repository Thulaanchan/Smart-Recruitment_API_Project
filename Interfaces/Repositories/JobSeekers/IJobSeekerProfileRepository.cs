using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers
{
    public interface IJobSeekerProfileRepository
    {
        Task<JobSeekerProfile?> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<bool> ExistsAsync(int jobSeekerId);

        Task<JobSeekerProfile> CreateAsync(JobSeekerProfile profile);

        Task<JobSeekerProfile> UpdateAsync(JobSeekerProfile profile);
    }
}