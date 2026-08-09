using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers
{
    public interface IJobSeekerProfileService
    {
        Task<JobSeekerProfileResponseDto?> GetProfileAsync(
            int jobSeekerId);

        Task<JobSeekerProfileResponseDto> CreateProfileAsync(
            int jobSeekerId,
            CreateJobSeekerProfileDto dto);

        Task<JobSeekerProfileResponseDto?> UpdateProfileAsync(
            int jobSeekerId,
            UpdateJobSeekerProfileDto dto);
    }
}