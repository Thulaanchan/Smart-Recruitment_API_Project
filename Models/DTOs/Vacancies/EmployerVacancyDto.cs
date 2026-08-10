namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies
{
    public class EmployerVacancyDto
    {
        public int VacancyId { get; set; }

        public int EmployerId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int ExperienceRequired { get; set; }

        public int RequiredEducationLevel { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public bool IsActive { get; set; }

        public int TotalApplications { get; set; }

        public List<int> SkillIds { get; set; } = new List<int>();

        public List<string> SkillNames { get; set; } = new List<string>();
    }
}