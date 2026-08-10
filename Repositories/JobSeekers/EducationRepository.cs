using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.JobSeekers
{
    public class EducationRepository : IEducationRepository
    {
        private readonly ApplicationDbContext _context;

        public EducationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Education>> GetByJobSeekerIdAsync(
            int jobSeekerId)
        {
            return await _context.Educations
                .Where(x => x.JobSeekerId == jobSeekerId)
                .OrderByDescending(x => x.EndDate)
                .ThenByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<Education?> GetByIdAsync(int id)
        {
            return await _context.Educations
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Education> CreateAsync(
            Education education)
        {
            await _context.Educations.AddAsync(education);

            await _context.SaveChangesAsync();

            return education;
        }

        public async Task UpdateAsync(
            Education education)
        {
            _context.Educations.Update(education);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            Education education)
        {
            _context.Educations.Remove(education);

            await _context.SaveChangesAsync();
        }
    }
}