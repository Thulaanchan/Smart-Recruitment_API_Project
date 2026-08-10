namespace SmartRecruitmentMatchingPlatform.API.Models.Entities
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public int JobSeekerId { get; set; }

        public int VacancyId { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
    }
}