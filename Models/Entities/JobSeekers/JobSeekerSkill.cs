namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers
{
    public class JobSeekerSkill
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public int SkillId { get; set; }

        public int ProficiencyLevel { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}