using SmartRecruitmentMatchingPlatform.API.Models.Entities.Skills;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Skills
{
    public interface ISkillRepository
    {
        Task<List<Skill>> GetAllAsync();

        Task<Skill?> GetByIdAsync(int skillId);

        Task<Skill?> GetByNameAsync(string skillName);

        Task<Skill> CreateAsync(Skill skill);
    }
}