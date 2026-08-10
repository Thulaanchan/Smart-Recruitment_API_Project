using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers
{
    public class JobSeekerSkillRepository : IJobSeekerSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public JobSeekerSkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobSeekerSkill>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.JobSeekerSkills
                .Where(js => js.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public async Task<JobSeekerSkill?> GetByIdAsync(int id)
        {
            return await _context.JobSeekerSkills
                .FirstOrDefaultAsync(js => js.Id == id);
        }

        public async Task<bool> ExistsAsync(
            int jobSeekerId,
            int skillId,
            int? excludeId = null)
        {
            return await _context.JobSeekerSkills
                .AnyAsync(js =>
                    js.JobSeekerId == jobSeekerId &&
                    js.SkillId == skillId &&
                    (!excludeId.HasValue || js.Id != excludeId.Value));
        }

        public async Task<JobSeekerSkill> CreateAsync(JobSeekerSkill jobSeekerSkill)
        {
            await _context.JobSeekerSkills.AddAsync(jobSeekerSkill);
            await _context.SaveChangesAsync();
            return jobSeekerSkill;
        }

        public async Task UpdateAsync(JobSeekerSkill jobSeekerSkill)
        {
            _context.JobSeekerSkills.Update(jobSeekerSkill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(JobSeekerSkill jobSeekerSkill)
        {
            _context.JobSeekerSkills.Remove(jobSeekerSkill);
            await _context.SaveChangesAsync();
        }
    }
}
