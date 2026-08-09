namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies{
    public class Company
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Industry { get; set; }

        public string? Website { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}