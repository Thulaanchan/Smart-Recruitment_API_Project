namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers
{
    public class Experience
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrentJob { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}