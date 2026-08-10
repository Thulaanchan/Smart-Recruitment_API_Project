using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Employers
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get Employer by Employer Id
        public async Task<Employer?> GetByIdAsync(int employerId)
        {
            return await _context
                .Set<Employer>()
                .FirstOrDefaultAsync(e => e.EmployerId == employerId);
        }

        // Get Employer by User Id
        public async Task<Employer?> GetByUserIdAsync(int userId)
        {
            return await _context
                .Set<Employer>()
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }

        // Get Employer profile
        public async Task<Employer?> GetProfileAsync(int employerId)
        {
            return await _context
                .Set<Employer>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployerId == employerId);
        }

        // Get all Employers
        public async Task<IEnumerable<Employer>> GetAllAsync()
        {
            return await _context
                .Set<Employer>()
                .AsNoTracking()
                .ToListAsync();
        }

        // Add new Employer
        public async Task AddAsync(Employer employer)
        {
            await _context
                .Set<Employer>()
                .AddAsync(employer);
        }

        // Update Employer
        public Task UpdateAsync(Employer employer)
        {
            _context
                .Set<Employer>()
                .Update(employer);

            return Task.CompletedTask;
        }

        // Check Employer exists using Employer Id
        public async Task<bool> ExistsAsync(int employerId)
        {
            return await _context
                .Set<Employer>()
                .AnyAsync(e => e.EmployerId == employerId);
        }

        // Check Employer exists using User Id
        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context
                .Set<Employer>()
                .AnyAsync(e => e.UserId == userId);
        }

        // Save database changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}