using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/cv")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerCVController : ControllerBase
    {
        private readonly ICVService _cvService;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerCVController(
            ICVService cvService,
            IJobSeekerRepository jobSeekerRepository)
        {
            _cvService = cvService;
            _jobSeekerRepository = jobSeekerRepository;
        }

        // POST: api/jobseekers/cv/{jobSeekerId}/upload
        [HttpPost("{jobSeekerId:int}/upload")]
        public async Task<IActionResult> UploadCV(
            int jobSeekerId,
            IFormFile file)
        {
            var authResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authResult != null)
            {
                return authResult;
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "Please select a CV file."
                });
            }

            // Maximum file size = 5 MB
            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
            {
                return BadRequest(new
                {
                    message = "CV file size cannot exceed 5 MB."
                });
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
                return BadRequest(new
                {
                    message =
                        "Only PDF, DOC and DOCX files are allowed."
                });
            }

            try
            {
                var result =
                    await _cvService.UploadCVAsync(
                        jobSeekerId,
                        file);

                return Ok(new
                {
                    message = "CV uploaded successfully.",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // GET: api/jobseekers/cv/{jobSeekerId}
        [HttpGet("{jobSeekerId:int}")]
        public async Task<IActionResult> GetCV(
            int jobSeekerId)
        {
            var authResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authResult != null)
            {
                return authResult;
            }

            var cv =
                await _cvService.GetCVAsync(jobSeekerId);

            if (cv == null)
            {
                return NotFound(new
                {
                    message = "CV not found."
                });
            }

            return Ok(cv);
        }

        // DELETE: api/jobseekers/cv/{jobSeekerId}
        [HttpDelete("{jobSeekerId:int}")]
        public async Task<IActionResult> DeleteCV(
            int jobSeekerId)
        {
            var authResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authResult != null)
            {
                return authResult;
            }

            var deleted =
                await _cvService.DeleteCVAsync(jobSeekerId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "CV not found."
                });
            }

            return Ok(new
            {
                message = "CV deleted successfully."
            });
        }

        private async Task<IActionResult?> AuthorizeJobSeekerAsync(int requestedJobSeekerId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new { message = "Invalid authenticated user." });
            }

            var currentJobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (currentJobSeeker == null || currentJobSeeker.Id != requestedJobSeekerId)
            {
                return Forbid();
            }

            return null;
        }
    }
}