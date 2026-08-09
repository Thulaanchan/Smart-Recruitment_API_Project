namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies
{
    public class EmployerVacancyDto
    {
        public int VacancyId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int ExperienceRequired { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public bool IsActive { get; set; }

        public int TotalApplications { get; set; }
    }
}