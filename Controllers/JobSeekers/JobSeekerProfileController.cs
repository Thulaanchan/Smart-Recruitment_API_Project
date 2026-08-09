using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/profile")]
    // [Authorize(Roles = "JobSeeker")]
    public class JobSeekerProfileController : ControllerBase
    {
        private readonly IJobSeekerProfileService _profileService;

        public JobSeekerProfileController(
            IJobSeekerProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{jobSeekerId:int}")]
        public async Task<IActionResult> GetProfile(int jobSeekerId)
        {
            var profile =
                await _profileService.GetProfileAsync(jobSeekerId);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });
            }

            return Ok(profile);
        }

        [HttpPost("{jobSeekerId:int}")]
        public async Task<IActionResult> CreateProfile(
            int jobSeekerId,
            [FromBody] CreateJobSeekerProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var profile =
                    await _profileService.CreateProfileAsync(
                        jobSeekerId,
                        dto);

                return CreatedAtAction(
                    nameof(GetProfile),
                    new { jobSeekerId },
                    profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{jobSeekerId:int}")]
        public async Task<IActionResult> UpdateProfile(
            int jobSeekerId,
            [FromBody] UpdateJobSeekerProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile =
                await _profileService.UpdateProfileAsync(
                    jobSeekerId,
                    dto);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });
            }

            return Ok(profile);
        }
    }
}