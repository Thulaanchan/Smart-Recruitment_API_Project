using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Applications;

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

        // Get all applications received for an Employer's vacancy
        public async Task<object> GetApplicationsByVacancyAsync(
            int vacancyId,
            int employerId)
        {
            // Make sure this vacancy belongs to the logged-in Employer
            var belongsToEmployer =
                await _vacancyRepository.BelongsToEmployerAsync(
                    vacancyId,
                    employerId);

            if (!belongsToEmployer)
            {
                return Array.Empty<object>();
            }

            var applications =
                await _applicationRepository
                    .GetByVacancyIdAsync(vacancyId);

            return applications;
        }

        // Get one application
        public async Task<object?> GetApplicationByIdAsync(
            int applicationId,
            int employerId)
        {
            var application =
                await _applicationRepository
                    .GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            // Check that the application belongs to
            // one of this Employer's vacancies
            var belongsToEmployer =
                await _vacancyRepository.BelongsToEmployerAsync(
                    application.VacancyId,
                    employerId);

            if (!belongsToEmployer)
            {
                return null;
            }

            return application;
        }

        // Update application status
        public async Task<bool> UpdateApplicationStatusAsync(
            int applicationId,
            string status,
            int employerId)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }

            var application =
                await _applicationRepository
                    .GetByIdAsync(applicationId);

            if (application == null)
            {
                return false;
            }

            // Security / ownership check
            var belongsToEmployer =
                await _vacancyRepository.BelongsToEmployerAsync(
                    application.VacancyId,
                    employerId);

            if (!belongsToEmployer)
            {
                return false;
            }

            application.Status = status;

            await _applicationRepository
                .UpdateAsync(application);

            await _applicationRepository
                .SaveChangesAsync();

            return true;
        }
    }
}