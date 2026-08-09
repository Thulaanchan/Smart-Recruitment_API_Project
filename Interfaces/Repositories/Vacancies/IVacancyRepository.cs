using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies
{
    public interface IVacancyRepository
    {
        Task<Vacancy?> GetByIdAsync(int vacancyId);

        Task<IEnumerable<Vacancy>> GetAllAsync();

        Task<IEnumerable<Vacancy>> GetByEmployerIdAsync(int employerId);

        Task AddAsync(Vacancy vacancy);

        Task UpdateAsync(Vacancy vacancy);

        Task<bool> ExistsAsync(int vacancyId);

        Task<bool> BelongsToEmployerAsync(
            int vacancyId,
            int employerId);

        Task SaveChangesAsync();
    }
}