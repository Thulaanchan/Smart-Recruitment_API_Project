namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Vacancies
{
    public interface IVacancyService
    {
        // Create a new vacancy for an employer
        Task<bool> CreateVacancyAsync(
            int employerId,
            object createData);

        // Get a vacancy by Id
        Task<object?> GetVacancyByIdAsync(
            int vacancyId);

        // Get all vacancies created by an employer
        Task<object> GetEmployerVacanciesAsync(
            int employerId);

        // Update an existing vacancy
        Task<bool> UpdateVacancyAsync(
            int vacancyId,
            int employerId,
            object updateData);

        // Close a vacancy
        Task<bool> CloseVacancyAsync(
            int vacancyId,
            int employerId);

        // Reopen a closed vacancy
        Task<bool> ReopenVacancyAsync(
            int vacancyId,
            int employerId);

        // Check whether vacancy exists
        Task<bool> ExistsAsync(
            int vacancyId);
    }
}