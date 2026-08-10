using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/{jobSeekerId:int}/experiences")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerExperienceController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerExperienceController(
            IExperienceService experienceService,
            IJobSeekerRepository jobSeekerRepository)
        {
            _experienceService = experienceService;
            _jobSeekerRepository = jobSeekerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetExperiences(
            int jobSeekerId)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            var experiences =
                await _experienceService
                    .GetByJobSeekerIdAsync(jobSeekerId);

            return Ok(experiences);
        }

        [HttpPost]
        public async Task<IActionResult> AddExperience(
            int jobSeekerId,
            [FromBody] AddExperienceDto dto)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            try
            {
                var experience =
                    await _experienceService
                        .AddAsync(jobSeekerId, dto);

                return CreatedAtAction(
                    nameof(GetExperiences),
                    new { jobSeekerId },
                    experience);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{experienceId:int}")]
        public async Task<IActionResult> UpdateExperience(
            int jobSeekerId,
            int experienceId,
            [FromBody] AddExperienceDto dto)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            try
            {
                var experience =
                    await _experienceService.UpdateAsync(
                        jobSeekerId,
                        experienceId,
                        dto);

                if (experience == null)
                {
                    return NotFound(new
                    {
                        message = "Experience not found."
                    });
                }

                return Ok(experience);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{experienceId:int}")]
        public async Task<IActionResult> DeleteExperience(
            int jobSeekerId,
            int experienceId)
        {
            var authorizationResult =
                await AuthorizeJobSeekerAsync(jobSeekerId);

            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            var deleted =
                await _experienceService.DeleteAsync(
                    jobSeekerId,
                    experienceId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Experience not found."
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