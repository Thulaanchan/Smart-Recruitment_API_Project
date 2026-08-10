using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        public Task<int> GetTotalUsersAsync()
        {
            return Task.FromResult(0);
        }

        public Task<int> GetTotalJobSeekersAsync()
        {
            return Task.FromResult(0);
        }

        public Task<int> GetTotalEmployersAsync()
        {
            return Task.FromResult(0);
        }

        public Task<int> GetTotalVacanciesAsync()
        {
            return Task.FromResult(0);
        }

        public Task<int> GetTotalApplicationsAsync()
        {
            return Task.FromResult(0);
        }
    }
}