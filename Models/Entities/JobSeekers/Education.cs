namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers
{
    public class Education
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public string Institution { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public string? FieldOfStudy { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}