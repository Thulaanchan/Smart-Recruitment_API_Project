namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Applications
{
    public interface IApplicationService
    {
        // Get applications received for a vacancy
        Task<object> GetApplicationsByVacancyAsync(
            int vacancyId,
            int employerId);

        // Get a specific application
        Task<object?> GetApplicationByIdAsync(
            int applicationId,
            int employerId);

        // Update application status
        Task<bool> UpdateApplicationStatusAsync(
            int applicationId,
            string status,
            int employerId);
    }
}