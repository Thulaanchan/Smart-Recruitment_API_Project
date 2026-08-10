namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching
{
    public class MatchResultDto
    {
        public MatchScoreDto MatchScore { get; set; }
            = new MatchScoreDto();

        public SkillGapDto SkillGap { get; set; }
            = new SkillGapDto();
    }
}