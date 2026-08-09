using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Applications
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(int applicationId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a =>
                    a.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<Application>> GetByVacancyIdAsync(
            int vacancyId)
        {
            return await _context.Applications
                .Where(a => a.VacancyId == vacancyId)
                .ToListAsync();
        }

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
        }

        public Task UpdateAsync(Application application)
        {
            _context.Applications.Update(application);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int applicationId)
        {
            return await _context.Applications
                .AnyAsync(a =>
                    a.ApplicationId == applicationId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}