using SmartRecruitmentMatchingPlatform.API.Models.Enums.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.ContactRequests
{
    public class ContactRequestDto
    {
        public int ContactRequestId { get; set; }

        public int EmployerId { get; set; }

        public string EmployerName { get; set; } = string.Empty;

        public int JobSeekerId { get; set; }

        public string JobSeekerName { get; set; } = string.Empty;

        public int? VacancyId { get; set; }

        public string? VacancyTitle { get; set; }

        public string? Message { get; set; }

        public ContactRequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RespondedAt { get; set; }
    }
}
