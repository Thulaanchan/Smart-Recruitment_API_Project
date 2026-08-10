using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalJobSeekersAsync()
        {
            return await _context.JobSeekers.CountAsync();
        }

        public async Task<int> GetTotalEmployersAsync()
        {
            return await _context.Employers.CountAsync();
        }

        public async Task<int> GetTotalVacanciesAsync()
        {
            return await _context.Vacancies.CountAsync();
        }

        public async Task<int> GetTotalApplicationsAsync()
        {
            return await _context.Applications.CountAsync();
        }
    }
}