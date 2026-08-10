using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;

// JWT Authentication module ready ஆன பிறகு இதை மீண்டும் use பண்ணலாம்
// using Microsoft.AspNetCore.Authorization;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/cv")]

    // TEMPORARY - Swagger testing
    // JWT setup முடிந்த பிறகு uncomment பண்ண வேண்டும்
    // [Authorize(Roles = "JobSeeker")]

    public class JobSeekerCVController : ControllerBase
    {
        private readonly ICVService _cvService;

        public JobSeekerCVController(ICVService cvService)
        {
            _cvService = cvService;
        }

        // POST: api/jobseekers/cv/{jobSeekerId}/upload
        [HttpPost("{jobSeekerId:int}/upload")]
        public async Task<IActionResult> UploadCV(
            int jobSeekerId,
            IFormFile file)
        {
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
    }
}