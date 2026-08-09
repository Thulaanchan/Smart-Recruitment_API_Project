using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Services.Employers
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(
            IEmployerRepository employerRepository)
        {
            _employerRepository = employerRepository;
        }

        // Get Employer profile
        public async Task<object?> GetProfileAsync(
            int employerId)
        {
            var employer =
                await _employerRepository
                    .GetProfileAsync(employerId);

            if (employer == null)
            {
                return null;
            }

            return employer;
        }

        // Get Employer using logged-in User Id
        public async Task<object?> GetByUserIdAsync(
            int userId)
        {
            var employer =
                await _employerRepository
                    .GetByUserIdAsync(userId);

            return employer;
        }

        // Get Employer dashboard
        public async Task<object> GetDashboardAsync(
            int employerId)
        {
            var employer =
                await _employerRepository
                    .GetByIdAsync(employerId);

            if (employer == null)
            {
                return new
                {
                    message = "Employer not found."
                };
            }

            // Dashboard counts will be connected later
            // with Vacancy/Application repositories.

            return new
            {
                employerId = employerId,
                totalVacancies = 0,
                activeVacancies = 0,
                closedVacancies = 0,
                totalApplications = 0,
                pendingApplications = 0,
                shortlistedApplications = 0,
                acceptedApplications = 0,
                rejectedApplications = 0
            };
        }

        // Update Employer basic information
        public async Task<bool> UpdateEmployerAsync(
            int employerId,
            object updateData)
        {
            if (updateData == null)
            {
                return false;
            }

            var employer =
                await _employerRepository
                    .GetByIdAsync(employerId);

            if (employer == null)
            {
                return false;
            }

            /*
             * Actual DTO -> Employer property mapping
             * will be added when UpdateEmployerDto
             * is finalized.
             */

            await _employerRepository
                .UpdateAsync(employer);

            await _employerRepository
                .SaveChangesAsync();

            return true;
        }

        // Check Employer exists
        public async Task<bool> ExistsAsync(
            int employerId)
        {
            return await _employerRepository
                .ExistsAsync(employerId);
        }
    }
}