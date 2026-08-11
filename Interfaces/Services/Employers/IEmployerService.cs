using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers
{
    public interface IEmployerService
    {
        // Get employer profile using Employer Id
        Task<EmployerProfileDto?> GetProfileAsync(int employerId);

        // Get employer profile using logged-in User Id
        Task<EmployerProfileDto?> GetByUserIdAsync(int userId);

        // Get employer dashboard information
        Task<object> GetDashboardAsync(int employerId);

        // Update employer basic information
        Task<EmployerProfileDto?> UpdateEmployerAsync(
            int employerId,
            UpdateEmployerProfileDto dto);

        // Check whether employer exists
        Task<bool> ExistsAsync(int employerId);
    }
}