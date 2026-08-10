using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers
{
    public interface IJobSeekerSkillRepository
    {
        Task<List<JobSeekerSkill>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<JobSeekerSkill?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(
            int jobSeekerId,
            int skillId,
            int? excludeId = null);

        Task<JobSeekerSkill> CreateAsync(JobSeekerSkill jobSeekerSkill);

        Task UpdateAsync(JobSeekerSkill jobSeekerSkill);

        Task DeleteAsync(JobSeekerSkill jobSeekerSkill);
    }
}
