using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers
{
    public class CVRepository : ICVRepository
    {
        private readonly ApplicationDbContext _context;

        public CVRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CV?> GetByJobSeekerIdAsync(
            int jobSeekerId)
        {
            return await _context.CVs
                .FirstOrDefaultAsync(
                    x => x.JobSeekerId == jobSeekerId);
        }

        public async Task<CV> CreateAsync(CV cv)
        {
            await _context.CVs.AddAsync(cv);

            await _context.SaveChangesAsync();

            return cv;
        }

        public async Task<CV> UpdateAsync(CV cv)
        {
            _context.CVs.Update(cv);

            await _context.SaveChangesAsync();

            return cv;
        }

        public async Task DeleteAsync(CV cv)
        {
            _context.CVs.Remove(cv);

            await _context.SaveChangesAsync();
        }
    }
}