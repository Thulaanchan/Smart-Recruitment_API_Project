using Microsoft.AspNetCore.Http;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers
{
    public interface ICVService
    {
        Task<CVResponseDto> UploadCVAsync(
            int jobSeekerId,
            IFormFile file);

        Task<CVResponseDto?> GetCVAsync(
            int jobSeekerId);

        Task<bool> DeleteCVAsync(
            int jobSeekerId);
    }
}