using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Services.Employers
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IApplicationRepository _applicationRepository;

        public EmployerService(
            IEmployerRepository employerRepository,
            IVacancyRepository vacancyRepository,
            IApplicationRepository applicationRepository)
        {
            _employerRepository = employerRepository;
            _vacancyRepository = vacancyRepository;
            _applicationRepository = applicationRepository;
        }

        // Get Employer profile
        public async Task<EmployerProfileDto?> GetProfileAsync(
            int employerId)
        {
            var employer = await _employerRepository.GetProfileAsync(employerId);
            if (employer == null)
            {
                return null;
            }

            return MapToDto(employer);
        }

        // Get Employer using logged-in User Id
        public async Task<EmployerProfileDto?> GetByUserIdAsync(
            int userId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return null;
            }

            return MapToDto(employer);
        }

        // Get Employer dashboard
        public async Task<object> GetDashboardAsync(
            int employerId)
        {
            var employer = await _employerRepository.GetByIdAsync(employerId);
            if (employer == null)
            {
                return new
                {
                    message = "Employer not found."
                };
            }

            var vacancies = (await _vacancyRepository.GetByEmployerIdAsync(employerId)).ToList();
            int totalVacancies = vacancies.Count;
            int activeVacancies = vacancies.Count(v => v.IsActive);
            int closedVacancies = vacancies.Count(v => !v.IsActive);

            int totalApps = 0;
            int pendingApps = 0;
            int shortlistedApps = 0;
            int acceptedApps = 0;
            int rejectedApps = 0;

            foreach (var vacancy in vacancies)
            {
                var apps = await _applicationRepository.GetByVacancyIdAsync(vacancy.VacancyId);
                foreach (var app in apps)
                {
                    totalApps++;
                    if (string.Equals(app.Status, "Pending", StringComparison.OrdinalIgnoreCase)) pendingApps++;
                    else if (string.Equals(app.Status, "Shortlisted", StringComparison.OrdinalIgnoreCase)) shortlistedApps++;
                    else if (string.Equals(app.Status, "Accepted", StringComparison.OrdinalIgnoreCase)) acceptedApps++;
                    else if (string.Equals(app.Status, "Rejected", StringComparison.OrdinalIgnoreCase)) rejectedApps++;
                }
            }

            return new
            {
                employerId = employerId,
                companyName = employer.CompanyName,
                totalVacancies = totalVacancies,
                activeVacancies = activeVacancies,
                closedVacancies = closedVacancies,
                totalApplications = totalApps,
                pendingApplications = pendingApps,
                shortlistedApplications = shortlistedApps,
                acceptedApplications = acceptedApps,
                rejectedApplications = rejectedApps
            };
        }

        // Update Employer basic information
        public async Task<EmployerProfileDto?> UpdateEmployerAsync(
            int employerId,
            UpdateEmployerProfileDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var employer = await _employerRepository.GetByIdAsync(employerId);
            if (employer == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                employer.CompanyName = dto.CompanyName.Trim();
            }
            employer.CompanyDescription = dto.CompanyDescription?.Trim();
            employer.Location = dto.Location?.Trim();
            employer.Website = dto.Website?.Trim();

            await _employerRepository.UpdateAsync(employer);
            await _employerRepository.SaveChangesAsync();

            return MapToDto(employer);
        }

        // Check Employer exists
        public async Task<bool> ExistsAsync(
            int employerId)
        {
            return await _employerRepository.ExistsAsync(employerId);
        }

        private static EmployerProfileDto MapToDto(SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers.Employer employer)
        {
            return new EmployerProfileDto
            {
                EmployerId = employer.EmployerId,
                UserId = employer.UserId,
                CompanyName = employer.CompanyName,
                CompanyDescription = employer.CompanyDescription,
                Location = employer.Location,
                Website = employer.Website
            };
        }
    }
}