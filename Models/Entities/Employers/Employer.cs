namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers
{
    public class Employer
    {
        public int EmployerId { get; set; }

        public int UserId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string? CompanyDescription { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }
    }
}