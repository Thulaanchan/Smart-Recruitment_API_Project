using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecruitmentMatchingPlatform.Models.Entities.Users
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        [NotMapped]
        public bool IsExpired =>
            DateTime.UtcNow >= ExpiresAt;

        [NotMapped]
        public bool IsActive =>
            RevokedAt == null && !IsExpired;
    }
}