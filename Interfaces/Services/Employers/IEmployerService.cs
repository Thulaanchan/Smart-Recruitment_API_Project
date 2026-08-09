namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers
{
    public interface IEmployerService
    {
        // Get employer profile using Employer Id
        Task<object?> GetProfileAsync(int employerId);

        // Get employer using logged-in User Id
        Task<object?> GetByUserIdAsync(int userId);

        // Get employer dashboard information
        Task<object> GetDashboardAsync(int employerId);

        // Update employer basic information
        Task<bool> UpdateEmployerAsync(
            int employerId,
            object updateData);

        // Check whether employer exists
        Task<bool> ExistsAsync(int employerId);
    }
}