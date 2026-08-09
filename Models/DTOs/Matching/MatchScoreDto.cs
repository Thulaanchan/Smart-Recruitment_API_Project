namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching
{
    public class MatchScoreDto
    {
        public int JobSeekerId { get; set; }

        public int VacancyId { get; set; }

        public double SkillScore { get; set; }

        public double ExperienceScore { get; set; }

        public double EducationScore { get; set; }

        public double LocationScore { get; set; }

        public double OverallScore { get; set; }
    }
}