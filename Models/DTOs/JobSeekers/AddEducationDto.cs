using System.ComponentModel.DataAnnotations;

namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class AddEducationDto
    {
        [Required]
        [MaxLength(150)]
        public string Institution { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Qualification { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? FieldOfStudy { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}