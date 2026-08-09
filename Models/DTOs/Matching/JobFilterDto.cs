namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching
{
    public class JobFilterDto
    {
        public string? Keyword { get; set; }

        public string? Location { get; set; }

        public List<string> Skills { get; set; }
            = new List<string>();

        public double? MinimumMatchScore { get; set; }
    }
}