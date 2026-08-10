using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Vacancies
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ApplicationDbContext _context;

        public VacancyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vacancy?> GetByIdAsync(int vacancyId)
        {
            return await _context
                .Vacancies
                .Include(v => v.VacancySkills)
                    .ThenInclude(vs => vs.Skill)
                .FirstOrDefaultAsync(v => v.VacancyId == vacancyId);
        }

        public async Task<IEnumerable<Vacancy>> GetAllAsync()
        {
            return await _context
                .Vacancies
                .Include(v => v.VacancySkills)
                    .ThenInclude(vs => vs.Skill)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Vacancy>> GetByEmployerIdAsync(
            int employerId)
        {
            return await _context
                .Vacancies
                .Include(v => v.VacancySkills)
                    .ThenInclude(vs => vs.Skill)
                .AsNoTracking()
                .Where(v => v.EmployerId == employerId)
                .ToListAsync();
        }

        public async Task AddAsync(Vacancy vacancy)
        {
            await _context
                .Set<Vacancy>()
                .AddAsync(vacancy);
        }

        public Task UpdateAsync(Vacancy vacancy)
        {
            _context
                .Set<Vacancy>()
                .Update(vacancy);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int vacancyId)
        {
            return await _context
                .Set<Vacancy>()
                .AnyAsync(v => v.VacancyId == vacancyId);
        }

        public async Task<bool> BelongsToEmployerAsync(
            int vacancyId,
            int employerId)
        {
            return await _context
                .Set<Vacancy>()
                .AnyAsync(v =>
                    v.VacancyId == vacancyId &&
                    v.EmployerId == employerId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}