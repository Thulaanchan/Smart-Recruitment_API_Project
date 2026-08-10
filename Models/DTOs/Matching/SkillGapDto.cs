namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching
{
    public class SkillGapDto
    {
        public int VacancyId { get; set; }

        public List<string> MatchedSkills { get; set; }
            = new List<string>();

        public List<string> MissingSkills { get; set; }
            = new List<string>();
    }
}