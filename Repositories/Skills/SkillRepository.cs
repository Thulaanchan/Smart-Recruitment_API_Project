using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Skills;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Skills;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Skills
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _context.Skills
                .AsNoTracking()
                .OrderBy(x => x.SkillName)
                .ToListAsync();
        }

        public async Task<Skill?> GetByIdAsync(int skillId)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(x => x.SkillId == skillId);
        }

        public async Task<Skill?> GetByNameAsync(string skillName)
        {
            var normalizedName =
                skillName.Trim().ToLower();

            return await _context.Skills
                .FirstOrDefaultAsync(
                    x => x.SkillName.ToLower() == normalizedName);
        }

        public async Task<Skill> CreateAsync(Skill skill)
        {
            await _context.Skills.AddAsync(skill);

            await _context.SaveChangesAsync();

            return skill;
        }
    }
}