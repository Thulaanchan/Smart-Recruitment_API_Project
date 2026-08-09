using SmartRecruitmentMatchingPlatform.API.Models.Entities;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers
{
    public interface IEmployerRepository
    {
        // Get employer using Employer Id
        Task<Employer?> GetByIdAsync(int employerId);

        // Get employer using logged-in User Id
        Task<Employer?> GetByUserIdAsync(int userId);

        // Get employer with profile/company information
        Task<Employer?> GetProfileAsync(int employerId);

        // Get all employers
        Task<IEnumerable<Employer>> GetAllAsync();

        // Add new employer
        Task AddAsync(Employer employer);

        // Update employer
        Task UpdateAsync(Employer employer);

        // Check whether employer exists
        Task<bool> ExistsAsync(int employerId);

        // Check whether an employer exists for a User
        Task<bool> ExistsByUserIdAsync(int userId);

        // Save database changes
        Task SaveChangesAsync();
    }
}