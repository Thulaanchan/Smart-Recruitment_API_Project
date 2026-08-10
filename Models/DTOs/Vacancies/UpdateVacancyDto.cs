namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies
{
    public class UpdateVacancyDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int ExperienceRequired { get; set; }

        public int RequiredEducationLevel { get; set; }

        public DateTime? ClosingDate { get; set; }

        public bool IsActive { get; set; } = true;

        public List<int> SkillIds { get; set; } = new List<int>();
    }
}