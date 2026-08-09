namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers
{
    public class JobSeekerProfile
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Location { get; set; }

        public string? Summary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}