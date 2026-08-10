using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Services.JobSeekers
{
    public class JobSeekerProfileService
        : IJobSeekerProfileService
    {
        private readonly IJobSeekerProfileRepository _repository;

        public JobSeekerProfileService(
            IJobSeekerProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<JobSeekerProfileResponseDto?>
            GetProfileAsync(int jobSeekerId)
        {
            var profile =
                await _repository.GetByJobSeekerIdAsync(
                    jobSeekerId);

            if (profile == null)
            {
                return null;
            }

            return MapToResponse(profile);
        }

        public async Task<JobSeekerProfileResponseDto>
            CreateProfileAsync(
                int jobSeekerId,
                CreateJobSeekerProfileDto dto)
        {
            var exists =
                await _repository.ExistsAsync(jobSeekerId);

            if (exists)
            {
                throw new InvalidOperationException(
                    "Job seeker profile already exists.");
            }

            var profile = new JobSeekerProfile
            {
                JobSeekerId = jobSeekerId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Location = dto.Location,
                Summary = dto.Summary,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(profile);

            return MapToResponse(profile);
        }

        public async Task<JobSeekerProfileResponseDto?>
            UpdateProfileAsync(
                int jobSeekerId,
                UpdateJobSeekerProfileDto dto)
        {
            var profile =
                await _repository.GetByJobSeekerIdAsync(
                    jobSeekerId);

            if (profile == null)
            {
                return null;
            }

            profile.FullName = dto.FullName;
            profile.PhoneNumber = dto.PhoneNumber;
            profile.Location = dto.Location;
            profile.Summary = dto.Summary;
            profile.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(profile);

            return MapToResponse(profile);
        }

        private static JobSeekerProfileResponseDto MapToResponse(
            JobSeekerProfile profile)
        {
            return new JobSeekerProfileResponseDto
            {
                Id = profile.Id,
                JobSeekerId = profile.JobSeekerId,
                FullName = profile.FullName,
                PhoneNumber = profile.PhoneNumber,
                Location = profile.Location,
                Summary = profile.Summary,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt
            };
        }
    }
}