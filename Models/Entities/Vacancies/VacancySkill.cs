using SmartRecruitmentMatchingPlatform.API.Models.Entities.Skills;

namespace SmartRecruitmentMatchingPlatform.API.Models.Entities
{
    public class VacancySkill
    {
        public int VacancySkillId { get; set; }

        public int VacancyId { get; set; }

        public int SkillId { get; set; }

        public Skill? Skill { get; set; }

        public Vacancy? Vacancy { get; set; }
    }
}