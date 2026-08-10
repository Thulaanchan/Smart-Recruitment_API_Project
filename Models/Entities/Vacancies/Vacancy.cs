using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Models.Entities
{
    public class Vacancy
    {
        public int VacancyId { get; set; }

        public int EmployerId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public decimal Salary { get; set; }

        public int ExperienceRequired { get; set; }

        public int RequiredEducationLevel { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ClosingDate { get; set; }

        public bool IsActive { get; set; } = true;

        public Employer? Employer { get; set; }

        public ICollection<VacancySkill> VacancySkills { get; set; } = new List<VacancySkill>();
    }
}