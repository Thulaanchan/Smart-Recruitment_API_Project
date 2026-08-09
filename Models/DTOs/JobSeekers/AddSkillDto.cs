using System.ComponentModel.DataAnnotations;

namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class AddSkillDto
    {
        [Required]
        public int SkillId { get; set; }

        [Range(1, 5)]
        public int ProficiencyLevel { get; set; } = 1;
    }
}