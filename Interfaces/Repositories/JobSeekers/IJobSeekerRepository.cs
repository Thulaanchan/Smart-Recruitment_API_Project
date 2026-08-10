using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers
{
    public interface IJobSeekerRepository
    {
        Task<JobSeeker?> GetByIdAsync(int id);

        Task<JobSeeker?> GetByUserIdAsync(int userId);

        Task<bool> ExistsAsync(int id);

        Task<JobSeeker> CreateAsync(JobSeeker jobSeeker);

        Task UpdateAsync(JobSeeker jobSeeker);
    }
}