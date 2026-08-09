using System.ComponentModel.DataAnnotations;

namespace SmartRecruitmentMatchingPlatform.Models.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}