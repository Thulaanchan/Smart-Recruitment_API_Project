namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Applications
{
    public class ApplicantDto
    {
        public int ApplicationId { get; set; }

        public int JobSeekerId { get; set; }

        public int VacancyId { get; set; }

        public string ApplicantName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? CVUrl { get; set; }

        public string ApplicationStatus { get; set; } = string.Empty;

        public DateTime AppliedDate { get; set; }
    }
}