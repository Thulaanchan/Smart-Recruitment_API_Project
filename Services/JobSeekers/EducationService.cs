using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Services.JobSeekers
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepository _educationRepository;

        public EducationService(
            IEducationRepository educationRepository)
        {
            _educationRepository = educationRepository;
        }

        public async Task<List<Education>> GetByJobSeekerIdAsync(
            int jobSeekerId)
        {
            return await _educationRepository
                .GetByJobSeekerIdAsync(jobSeekerId);
        }

        public async Task<Education?> GetByIdAsync(
            int id)
        {
            return await _educationRepository
                .GetByIdAsync(id);
        }

        public async Task<Education> AddAsync(
            int jobSeekerId,
            AddEducationDto dto)
        {
            ValidateDates(dto);

            var education = new Education
            {
                JobSeekerId = jobSeekerId,
                Institution = dto.Institution.Trim(),
                Qualification = dto.Qualification.Trim(),
                FieldOfStudy = dto.FieldOfStudy?.Trim(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            return await _educationRepository
                .CreateAsync(education);
        }

        public async Task<Education?> UpdateAsync(
            int jobSeekerId,
            int educationId,
            AddEducationDto dto)
        {
            ValidateDates(dto);

            var education = await _educationRepository
                .GetByIdAsync(educationId);

            if (education == null ||
                education.JobSeekerId != jobSeekerId)
            {
                return null;
            }

            education.Institution =
                dto.Institution.Trim();

            education.Qualification =
                dto.Qualification.Trim();

            education.FieldOfStudy =
                dto.FieldOfStudy?.Trim();

            education.StartDate =
                dto.StartDate;

            education.EndDate =
                dto.EndDate;

            await _educationRepository
                .UpdateAsync(education);

            return education;
        }

        public async Task<bool> DeleteAsync(
            int jobSeekerId,
            int educationId)
        {
            var education = await _educationRepository
                .GetByIdAsync(educationId);

            if (education == null ||
                education.JobSeekerId != jobSeekerId)
            {
                return false;
            }

            await _educationRepository
                .DeleteAsync(education);

            return true;
        }

        private static void ValidateDates(
            AddEducationDto dto)
        {
            if (dto.StartDate.HasValue &&
                dto.EndDate.HasValue &&
                dto.EndDate.Value < dto.StartDate.Value)
            {
                throw new ArgumentException(
                    "End date cannot be earlier than start date.");
            }
        }
    }
}