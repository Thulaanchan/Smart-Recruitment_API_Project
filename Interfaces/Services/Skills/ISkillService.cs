using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Skills;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Skills
{
    public interface ISkillService
    {
        Task<List<SkillDto>> GetAllAsync();

        Task<SkillDto?> GetByIdAsync(int skillId);

        Task<SkillDto> CreateAsync(CreateSkillDto dto);
    }
}
