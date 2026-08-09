namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies
{
    public class CompanySocialMedia
    {
        public int CompanySocialMediaId { get; set; }

        public int CompanyId { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? FacebookUrl { get; set; }

        public string? TwitterUrl { get; set; }

        public string? InstagramUrl { get; set; }

        public string? YouTubeUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Property
        public Company? Company { get; set; }
    }
}