namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class JobSeekerProfileResponseDto
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Location { get; set; }

        public string? Summary { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}