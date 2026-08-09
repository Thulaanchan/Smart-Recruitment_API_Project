namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies
{
    public class CompanyProfile
    {
        public int CompanyProfileId { get; set; }

        public int CompanyId { get; set; }

        public string? AboutCompany { get; set; }

        public string? Mission { get; set; }

        public string? Vision { get; set; }

        public string? Benefits { get; set; }

        public string? WorkingHours { get; set; }

        public string? CompanyCulture { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public Company? Company { get; set; }
    }
}