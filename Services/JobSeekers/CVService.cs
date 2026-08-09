using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Services.JobSeekers
{
    public class CVService : ICVService
    {
        private readonly ICVRepository _cvRepository;
        private readonly IWebHostEnvironment _environment;

        public CVService(
            ICVRepository cvRepository,
            IWebHostEnvironment environment)
        {
            _cvRepository = cvRepository;
            _environment = environment;
        }

        public async Task<CVResponseDto> UploadCVAsync(
            int jobSeekerId,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException(
                    "Please select a CV file.");
            }

            var extension =
                Path.GetExtension(file.FileName)
                    .ToLowerInvariant();

            var allowedExtensions = new[]
            {
                ".pdf",
                ".doc",
                ".docx"
            };

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException(
                    "Only PDF, DOC and DOCX files are allowed.");
            }

            const long maxFileSize =
                5 * 1024 * 1024;

            if (file.Length > maxFileSize)
            {
                throw new InvalidOperationException(
                    "CV file size cannot exceed 5 MB.");
            }

            var storageFolder = Path.Combine(
                _environment.ContentRootPath,
                "Storage",
                "CVs");

            Directory.CreateDirectory(storageFolder);

            var existingCV =
                await _cvRepository
                    .GetByJobSeekerIdAsync(jobSeekerId);

            if (existingCV != null &&
                File.Exists(existingCV.FilePath))
            {
                File.Delete(existingCV.FilePath);
            }

            var storedFileName =
                $"{jobSeekerId}_{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(
                storageFolder,
                storedFileName);

            await using (var stream =
                new FileStream(
                    filePath,
                    FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (existingCV == null)
            {
                var cv = new CV
                {
                    JobSeekerId = jobSeekerId,
                    OriginalFileName = file.FileName,
                    StoredFileName = storedFileName,
                    FilePath = filePath,
                    ContentType =
                        file.ContentType ?? string.Empty,
                    FileSize = file.Length,
                    UploadedAt = DateTime.UtcNow
                };

                await _cvRepository.CreateAsync(cv);

                return MapToResponse(cv);
            }

            existingCV.OriginalFileName =
                file.FileName;

            existingCV.StoredFileName =
                storedFileName;

            existingCV.FilePath =
                filePath;

            existingCV.ContentType =
                file.ContentType ?? string.Empty;

            existingCV.FileSize =
                file.Length;

            existingCV.UploadedAt =
                DateTime.UtcNow;

            await _cvRepository.UpdateAsync(
                existingCV);

            return MapToResponse(existingCV);
        }

        public async Task<CVResponseDto?> GetCVAsync(
            int jobSeekerId)
        {
            var cv =
                await _cvRepository
                    .GetByJobSeekerIdAsync(jobSeekerId);

            if (cv == null)
            {
                return null;
            }

            return MapToResponse(cv);
        }

        public async Task<bool> DeleteCVAsync(
            int jobSeekerId)
        {
            var cv =
                await _cvRepository
                    .GetByJobSeekerIdAsync(jobSeekerId);

            if (cv == null)
            {
                return false;
            }

            if (File.Exists(cv.FilePath))
            {
                File.Delete(cv.FilePath);
            }

            await _cvRepository.DeleteAsync(cv);

            return true;
        }

        private static CVResponseDto MapToResponse(
            CV cv)
        {
            return new CVResponseDto
            {
                Id = cv.Id,
                JobSeekerId = cv.JobSeekerId,
                OriginalFileName =
                    cv.OriginalFileName,
                StoredFileName =
                    cv.StoredFileName,
                ContentType =
                    cv.ContentType,
                FileSize =
                    cv.FileSize,
                UploadedAt =
                    cv.UploadedAt
            };
        }
    }
}