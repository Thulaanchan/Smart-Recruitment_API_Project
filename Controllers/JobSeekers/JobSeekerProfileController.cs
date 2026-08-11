using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/profile")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerProfileController : ControllerBase
    {
        private readonly IJobSeekerProfileService _profileService;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerProfileController(
            IJobSeekerProfileService profileService,
            IJobSeekerRepository jobSeekerRepository)
        {
            _profileService = profileService;
            _jobSeekerRepository = jobSeekerRepository;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var jobSeeker = await GetCurrentJobSeekerAsync();
            if (jobSeeker == null)
            {
                return Unauthorized(new { message = "Invalid job seeker identity." });
            }

            var profile = await _profileService.GetProfileAsync(jobSeeker.Id);
            if (profile == null)
            {
                return NotFound(new { message = "Job seeker profile not found." });
            }

            return Ok(profile);
        }

        [HttpGet("{jobSeekerId:int}")]
        public async Task<IActionResult> GetProfile(int jobSeekerId)
        {
            var authResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authResult != null)
            {
                return authResult;
            }

            var profile = await _profileService.GetProfileAsync(jobSeekerId);
            if (profile == null)
            {
                return NotFound(new { message = "Job seeker profile not found." });
            }

            return Ok(profile);
        }

        [HttpPost("{jobSeekerId:int}")]
        public async Task<IActionResult> CreateProfile(
            int jobSeekerId,
            [FromBody] CreateJobSeekerProfileDto dto)
        {
            var authResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authResult != null)
            {
                return authResult;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var profile = await _profileService.CreateProfileAsync(jobSeekerId, dto);
                return CreatedAtAction(nameof(GetProfile), new { jobSeekerId }, profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{jobSeekerId:int}")]
        public async Task<IActionResult> UpdateProfile(
            int jobSeekerId,
            [FromBody] UpdateJobSeekerProfileDto dto)
        {
            var authResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authResult != null)
            {
                return authResult;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = await _profileService.UpdateProfileAsync(jobSeekerId, dto);
            if (profile == null)
            {
                return NotFound(new { message = "Job seeker profile not found." });
            }

            return Ok(profile);
        }

        private async Task<SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.JobSeeker?> GetCurrentJobSeekerAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return null;
            }
            return await _jobSeekerRepository.GetByUserIdAsync(userId);
        }

        private async Task<IActionResult?> AuthorizeJobSeekerAsync(int requestedJobSeekerId)
        {
            var current = await GetCurrentJobSeekerAsync();
            if (current == null)
            {
                return Unauthorized(new { message = "Invalid authenticated user." });
            }

            if (current.Id != requestedJobSeekerId)
            {
                return Forbid();
            }

            return null;
        }
    }
}