namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.ContactRequests
{
    public class CreateContactRequestDto
    {
        public int JobSeekerId { get; set; }

        public int? VacancyId { get; set; }

        public string? Message { get; set; }
    }
}
