using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/{jobSeekerId:int}/educations")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerEducationController : ControllerBase
    {
        private readonly IEducationService _educationService;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerEducationController(
            IEducationService educationService,
            IJobSeekerRepository jobSeekerRepository)
        {
            _educationService = educationService;
            _jobSeekerRepository = jobSeekerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEducations(
            int jobSeekerId)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            var educations =
                await _educationService
                    .GetByJobSeekerIdAsync(jobSeekerId);

            return Ok(educations);
        }

        [HttpPost]
        public async Task<IActionResult> AddEducation(
            int jobSeekerId,
            [FromBody] AddEducationDto dto)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            try
            {
                var education =
                    await _educationService
                        .AddAsync(jobSeekerId, dto);

                return CreatedAtAction(
                    nameof(GetEducations),
                    new { jobSeekerId },
                    education);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{educationId:int}")]
        public async Task<IActionResult> UpdateEducation(
            int jobSeekerId,
            int educationId,
            [FromBody] AddEducationDto dto)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            try
            {
                var education =
                    await _educationService.UpdateAsync(
                        jobSeekerId,
                        educationId,
                        dto);

                if (education == null)
                {
                    return NotFound(new
                    {
                        message = "Education record not found."
                    });
                }

                return Ok(education);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{educationId:int}")]
        public async Task<IActionResult> DeleteEducation(
            int jobSeekerId,
            int educationId)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            var deleted =
                await _educationService.DeleteAsync(
                    jobSeekerId,
                    educationId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Education record not found."
                });
            }

            return NoContent();
        }

        private async Task<IActionResult?> AuthorizeJobSeekerAsync(
            int requestedJobSeekerId)
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid authenticated user."
                });
            }

            var currentJobSeeker =
                await _jobSeekerRepository
                    .GetByUserIdAsync(userId);

            if (currentJobSeeker == null)
            {
                return Forbid();
            }

            if (currentJobSeeker.Id != requestedJobSeekerId)
            {
                return Forbid();
            }

            return null;
        }
    }
}