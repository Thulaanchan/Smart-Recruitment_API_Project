namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Employers
{
    public class UpdateEmployerProfileDto
    {
        public string CompanyName { get; set; } = string.Empty;

        public string? CompanyDescription { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }
    }
}