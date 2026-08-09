namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching
{
    public class JobSearchResultDto
    {
        public int VacancyId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string? Location { get; set; }

        public List<string> RequiredSkills { get; set; }
            = new List<string>();

        public double MatchScore { get; set; }
    }
}