using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int applicationId);

        Task<IEnumerable<Application>> GetByVacancyIdAsync(int vacancyId);

        Task AddAsync(Application application);

        Task UpdateAsync(Application application);

        Task<bool> ExistsAsync(int applicationId);

        Task SaveChangesAsync();
    }
}