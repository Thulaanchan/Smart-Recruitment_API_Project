using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Skills;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Skills;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Skills;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Skills;

namespace SmartRecruitmentMatchingPlatform.API.Services.Skills
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<List<SkillDto>> GetAllAsync()
        {
            var skills = await _skillRepository.GetAllAsync();
            return skills.Select(s => new SkillDto
            {
                SkillId = s.SkillId,
                SkillName = s.SkillName
            }).ToList();
        }

        public async Task<SkillDto?> GetByIdAsync(int skillId)
        {
            if (skillId <= 0)
            {
                return null;
            }

            var skill = await _skillRepository.GetByIdAsync(skillId);
            if (skill == null)
            {
                return null;
            }

            return new SkillDto
            {
                SkillId = skill.SkillId,
                SkillName = skill.SkillName
            };
        }

        public async Task<SkillDto> CreateAsync(CreateSkillDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.SkillName))
            {
                throw new ArgumentException("Skill name cannot be empty.");
            }

            var trimmedName = dto.SkillName.Trim();

            var existingSkill = await _skillRepository.GetByNameAsync(trimmedName);
            if (existingSkill != null)
            {
                throw new InvalidOperationException($"Skill with name '{trimmedName}' already exists.");
            }

            var skill = new Skill
            {
                SkillName = trimmedName
            };

            var created = await _skillRepository.CreateAsync(skill);

            return new SkillDto
            {
                SkillId = created.SkillId,
                SkillName = created.SkillName
            };
        }
    }
}
