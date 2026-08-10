using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly ApplicationDbContext _context;

        public ExperienceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Experience>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Experiences
                .Where(x => x.JobSeekerId == jobSeekerId)
                .OrderByDescending(x => x.IsCurrentJob)
                .ThenByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<Experience?> GetByIdAsync(int id)
        {
            return await _context.Experiences
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Experience> CreateAsync(Experience experience)
        {
            await _context.Experiences.AddAsync(experience);
            await _context.SaveChangesAsync();

            return experience;
        }

        public async Task UpdateAsync(Experience experience)
        {
            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Experience experience)
        {
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
        }
    }
}