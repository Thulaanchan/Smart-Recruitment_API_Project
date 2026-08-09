using System.ComponentModel.DataAnnotations;

namespace SmartRecruitmentMatchingPlatform.Models.DTOs.Users
{
    public class UpdateUserDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }
    }
}