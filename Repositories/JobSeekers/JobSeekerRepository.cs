using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobSeeker?> GetByIdAsync(int id)
        {
            return await _context.JobSeekers
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<JobSeeker?> GetByUserIdAsync(int userId)
        {
            return await _context.JobSeekers
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.JobSeekers
                .AnyAsync(x => x.Id == id);
        }

        public async Task<JobSeeker> CreateAsync(
            JobSeeker jobSeeker)
        {
            await _context.JobSeekers.AddAsync(jobSeeker);

            await _context.SaveChangesAsync();

            return jobSeeker;
        }

        public async Task UpdateAsync(
            JobSeeker jobSeeker)
        {
            _context.JobSeekers.Update(jobSeeker);

            await _context.SaveChangesAsync();
        }
    }
}