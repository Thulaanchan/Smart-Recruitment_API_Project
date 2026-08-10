using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Services.JobSeekers
{
    public class ExperienceService : IExperienceService
    {
        private readonly IExperienceRepository _experienceRepository;

        public ExperienceService(
            IExperienceRepository experienceRepository)
        {
            _experienceRepository = experienceRepository;
        }

        public async Task<List<Experience>> GetByJobSeekerIdAsync(
            int jobSeekerId)
        {
            return await _experienceRepository
                .GetByJobSeekerIdAsync(jobSeekerId);
        }

        public async Task<Experience?> GetByIdAsync(int id)
        {
            return await _experienceRepository.GetByIdAsync(id);
        }

        public async Task<Experience> AddAsync(
            int jobSeekerId,
            AddExperienceDto dto)
        {
            ValidateDates(dto);

            var experience = new Experience
            {
                JobSeekerId = jobSeekerId,
                JobTitle = dto.JobTitle.Trim(),
                CompanyName = dto.CompanyName.Trim(),
                Description = dto.Description?.Trim(),
                StartDate = dto.StartDate,
                EndDate = dto.IsCurrentJob
                    ? null
                    : dto.EndDate,
                IsCurrentJob = dto.IsCurrentJob
            };

            return await _experienceRepository.CreateAsync(experience);
        }

        public async Task<Experience?> UpdateAsync(
            int jobSeekerId,
            int experienceId,
            AddExperienceDto dto)
        {
            ValidateDates(dto);

            var experience =
                await _experienceRepository.GetByIdAsync(experienceId);

            if (experience == null ||
                experience.JobSeekerId != jobSeekerId)
            {
                return null;
            }

            experience.JobTitle = dto.JobTitle.Trim();
            experience.CompanyName = dto.CompanyName.Trim();
            experience.Description = dto.Description?.Trim();
            experience.StartDate = dto.StartDate;
            experience.EndDate = dto.IsCurrentJob
                ? null
                : dto.EndDate;
            experience.IsCurrentJob = dto.IsCurrentJob;

            await _experienceRepository.UpdateAsync(experience);

            return experience;
        }

        public async Task<bool> DeleteAsync(
            int jobSeekerId,
            int experienceId)
        {
            var experience =
                await _experienceRepository.GetByIdAsync(experienceId);

            if (experience == null ||
                experience.JobSeekerId != jobSeekerId)
            {
                return false;
            }

            await _experienceRepository.DeleteAsync(experience);

            return true;
        }

        private static void ValidateDates(AddExperienceDto dto)
        {
            if (dto.StartDate.HasValue &&
                dto.EndDate.HasValue &&
                !dto.IsCurrentJob &&
                dto.EndDate.Value < dto.StartDate.Value)
            {
                throw new ArgumentException(
                    "End date cannot be earlier than start date.");
            }
        }
    }
}