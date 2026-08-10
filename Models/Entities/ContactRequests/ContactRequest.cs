using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Enums.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.ContactRequests
{
    public class ContactRequest
    {
        public int ContactRequestId { get; set; }

        // Employer who sends the request
        public int EmployerId { get; set; }

        // Job seeker who receives the request
        public int JobSeekerId { get; set; }

        // Optional vacancy connected to this request
        public int? VacancyId { get; set; }

        public string? Message { get; set; }

        public ContactRequestStatus Status { get; set; }
            = ContactRequestStatus.Pending;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }

        // Navigation properties
        public Employer? Employer { get; set; }

        public JobSeeker? JobSeeker { get; set; }

        public Vacancy? Vacancy { get; set; }
    }
}