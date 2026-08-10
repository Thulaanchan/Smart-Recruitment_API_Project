using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers
{
    public class JobSeekerProfileRepository
        : IJobSeekerProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerProfileRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerProfile?>
            GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(
                    x => x.JobSeekerId == jobSeekerId);
        }

        public async Task<bool> ExistsAsync(int jobSeekerId)
        {
            return await _context.JobSeekerProfiles
                .AnyAsync(
                    x => x.JobSeekerId == jobSeekerId);
        }

        public async Task<JobSeekerProfile> CreateAsync(
            JobSeekerProfile profile)
        {
            await _context.JobSeekerProfiles.AddAsync(profile);

            await _context.SaveChangesAsync();

            return profile;
        }

        public async Task<JobSeekerProfile> UpdateAsync(
            JobSeekerProfile profile)
        {
            _context.JobSeekerProfiles.Update(profile);

            await _context.SaveChangesAsync();

            return profile;
        }
    }
}