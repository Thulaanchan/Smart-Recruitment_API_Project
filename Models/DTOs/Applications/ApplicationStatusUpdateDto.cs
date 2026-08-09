namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Applications
{
    public class ApplicationStatusUpdateDto
    {
        public int ApplicationId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}