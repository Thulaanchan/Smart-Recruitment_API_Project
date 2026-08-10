using Microsoft.AspNetCore.Http;

namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers
{
    public class UploadCVDto
    {
        public IFormFile File { get; set; } = null!;
    }
}