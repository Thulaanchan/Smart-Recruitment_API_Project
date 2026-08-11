using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Vacancies
{
    public interface IVacancyService
    {
        Task<EmployerVacancyDto?> CreateVacancyAsync(
            int employerId,
            CreateVacancyDto dto);

        Task<EmployerVacancyDto?> GetVacancyByIdAsync(
            int vacancyId);

        Task<IEnumerable<EmployerVacancyDto>> GetEmployerVacanciesAsync(
            int employerId);

        Task<IEnumerable<EmployerVacancyDto>> GetAllVacanciesAsync();

        Task<IEnumerable<EmployerVacancyDto>> SearchVacanciesAsync(
            string? keyword,
            string? location,
            string? skills);

        Task<bool> UpdateVacancyAsync(
            int vacancyId,
            int employerId,
            UpdateVacancyDto dto);

        Task<bool> CloseVacancyAsync(
            int vacancyId,
            int employerId);

        Task<bool> ReopenVacancyAsync(
            int vacancyId,
            int employerId);

        Task<bool> ExistsAsync(
            int vacancyId);
    }
}