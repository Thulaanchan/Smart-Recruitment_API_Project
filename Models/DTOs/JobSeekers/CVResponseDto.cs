namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class CVResponseDto
    {
        public int Id { get; set; }

        public int JobSeekerId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}