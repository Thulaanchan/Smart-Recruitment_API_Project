using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.JobSeekers
{
    [ApiController]
    [Route("api/jobseekers/{jobSeekerId:int}/skills")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerSkillController : ControllerBase
    {
        private readonly IJobSeekerSkillService _jobSeekerSkillService;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerSkillController(
            IJobSeekerSkillService jobSeekerSkillService,
            IJobSeekerRepository jobSeekerRepository)
        {
            _jobSeekerSkillService = jobSeekerSkillService;
            _jobSeekerRepository = jobSeekerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills(int jobSeekerId)
        {
            var authorizationResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            var skills = await _jobSeekerSkillService.GetByJobSeekerIdAsync(jobSeekerId);
            return Ok(skills);
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill(
            int jobSeekerId,
            [FromBody] AddSkillDto dto)
        {
            var authorizationResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _jobSeekerSkillService.AddAsync(jobSeekerId, dto);
                return CreatedAtAction(
                    nameof(GetSkills),
                    new { jobSeekerId },
                    created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{jobSeekerSkillId:int}")]
        public async Task<IActionResult> UpdateSkill(
            int jobSeekerId,
            int jobSeekerSkillId,
            [FromBody] AddSkillDto dto)
        {
            var authorizationResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _jobSeekerSkillService.UpdateAsync(
                    jobSeekerId,
                    jobSeekerSkillId,
                    dto);

                if (updated == null)
                {
                    return NotFound(new { message = "JobSeeker skill record not found." });
                }

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{jobSeekerSkillId:int}")]
        public async Task<IActionResult> DeleteSkill(
            int jobSeekerId,
            int jobSeekerSkillId)
        {
            var authorizationResult = await AuthorizeJobSeekerAsync(jobSeekerId);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            var deleted = await _jobSeekerSkillService.DeleteAsync(
                jobSeekerId,
                jobSeekerSkillId);

            if (!deleted)
            {
                return NotFound(new { message = "JobSeeker skill record not found." });
            }

            return NoContent();
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
