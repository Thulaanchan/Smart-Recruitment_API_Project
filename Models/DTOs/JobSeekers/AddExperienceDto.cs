using System.ComponentModel.DataAnnotations;

namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class AddExperienceDto
    {
        [Required]
        [MaxLength(150)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCurrentJob { get; set; }
    }
}