using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Applications;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Applications;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IEmployerRepository _employerRepository;

        public ApplicationController(
            IApplicationService applicationService,
            IJobSeekerRepository jobSeekerRepository,
            IEmployerRepository employerRepository)
        {
            _applicationService = applicationService;
            _jobSeekerRepository = jobSeekerRepository;
            _employerRepository = employerRepository;
        }

        // POST: api/application/apply/5
        [Authorize(Roles = "JobSeeker")]
        [HttpPost("apply/{vacancyId:int}")]
        public async Task<IActionResult> Apply(int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return BadRequest(new { message = "Invalid vacancy ID." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                return StatusCode(403, new { message = "Job seeker profile not found for current user." });
            }

            var (success, message, application) = await _applicationService.ApplyAsync(jobSeeker.Id, vacancyId);
            if (!success)
            {
                if (message.Contains("already applied"))
                {
                    return Conflict(new { message });
                }
                return BadRequest(new { message });
            }

            return Ok(new { message, application });
        }

        // GET: api/application/my-applications
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                return StatusCode(403, new { message = "Job seeker profile not found." });
            }

            var apps = await _applicationService.GetJobSeekerApplicationsAsync(jobSeeker.Id);
            return Ok(apps);
        }

        // GET: api/application/vacancy/5
        [Authorize(Roles = "Employer")]
        [HttpGet("vacancy/{vacancyId:int}")]
        public async Task<IActionResult> GetVacancyApplications(int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return BadRequest(new { message = "Invalid vacancy ID." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return StatusCode(403, new { message = "Employer profile not found." });
            }

            var apps = await _applicationService.GetApplicationsByVacancyAsync(vacancyId, employer.EmployerId);
            return Ok(apps);
        }

        // GET: api/application/5
        [Authorize(Roles = "Employer")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetApplicationById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid application ID." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return StatusCode(403, new { message = "Employer profile not found." });
            }

            var app = await _applicationService.GetApplicationByIdAsync(id, employer.EmployerId);
            if (app == null)
            {
                return NotFound(new { message = "Application not found or not owned by employer." });
            }

            return Ok(app);
        }

        // PUT: api/application/5/status
        [Authorize(Roles = "Employer")]
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ApplicationStatusUpdateDto dto)
        {
            if (id <= 0 || dto == null || string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return StatusCode(403, new { message = "Employer profile not found." });
            }

            var updated = await _applicationService.UpdateApplicationStatusAsync(id, dto.Status, employer.EmployerId);
            if (!updated)
            {
                return NotFound(new { message = "Application not found or unauthorized to update status." });
            }

            return Ok(new { message = "Application status updated successfully." });
        }
    }
}