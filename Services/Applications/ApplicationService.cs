using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Applications;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Services.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IVacancyRepository _vacancyRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IVacancyRepository vacancyRepository)
        {
            _applicationRepository = applicationRepository;
            _vacancyRepository = vacancyRepository;
        }

        public async Task<(bool Success, string Message, Application? Application)> ApplyAsync(
            int jobSeekerId,
            int vacancyId)
        {
            if (jobSeekerId <= 0 || vacancyId <= 0)
            {
                return (false, "Invalid job seeker or vacancy ID.", null);
            }

            var vacancyExists = await _vacancyRepository.ExistsAsync(vacancyId);
            if (!vacancyExists)
            {
                return (false, "Vacancy not found.", null);
            }

            var hasAlreadyApplied = await _applicationRepository.HasAppliedAsync(jobSeekerId, vacancyId);
            if (hasAlreadyApplied)
            {
                return (false, "Job seeker has already applied to this vacancy.", null);
            }

            var application = new Application
            {
                JobSeekerId = jobSeekerId,
                VacancyId = vacancyId,
                AppliedDate = DateTime.UtcNow,
                Status = "Pending"
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return (true, "Application submitted successfully.", application);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByVacancyAsync(
            int vacancyId,
            int employerId)
        {
            var belongsToEmployer = await _vacancyRepository.BelongsToEmployerAsync(vacancyId, employerId);
            if (!belongsToEmployer)
            {
                return Enumerable.Empty<Application>();
            }

            return await _applicationRepository.GetByVacancyIdAsync(vacancyId);
        }

        public async Task<IEnumerable<Application>> GetJobSeekerApplicationsAsync(int jobSeekerId)
        {
            if (jobSeekerId <= 0)
            {
                return Enumerable.Empty<Application>();
            }

            return await _applicationRepository.GetByJobSeekerIdAsync(jobSeekerId);
        }

        public async Task<Application?> GetApplicationByIdAsync(
            int applicationId,
            int employerId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                return null;
            }

            var belongsToEmployer = await _vacancyRepository.BelongsToEmployerAsync(application.VacancyId, employerId);
            if (!belongsToEmployer)
            {
                return null;
            }

            return application;
        }

        public async Task<bool> UpdateApplicationStatusAsync(
            int applicationId,
            string status,
            int employerId)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }

            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                return false;
            }

            var belongsToEmployer = await _vacancyRepository.BelongsToEmployerAsync(application.VacancyId, employerId);
            if (!belongsToEmployer)
            {
                return false;
            }

            application.Status = status;

            await _applicationRepository.UpdateAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return true;
        }
    }
}