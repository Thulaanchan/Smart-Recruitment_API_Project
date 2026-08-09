using System.ComponentModel.DataAnnotations;

namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class CreateJobSeekerProfileDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(1000)]
        public string? Summary { get; set; }
    }
}