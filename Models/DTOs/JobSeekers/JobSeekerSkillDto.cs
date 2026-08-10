namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class JobSeekerSkillDto
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public int SkillId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public int ProficiencyLevel { get; set; }
    }
}