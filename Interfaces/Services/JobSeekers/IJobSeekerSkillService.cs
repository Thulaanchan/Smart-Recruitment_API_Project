using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers
{
    public interface IJobSeekerSkillService
    {
        Task<List<JobSeekerSkillDto>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task<JobSeekerSkillDto> AddAsync(int jobSeekerId, AddSkillDto dto);

        Task<JobSeekerSkillDto?> UpdateAsync(
            int jobSeekerId,
            int jobSeekerSkillId,
            AddSkillDto dto);

        Task<bool> DeleteAsync(int jobSeekerId, int jobSeekerSkillId);
    }
}
