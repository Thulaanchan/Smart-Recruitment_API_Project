using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.ContactRequests
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactRequestController : ControllerBase
    {
        private readonly IContactRequestService _contactRequestService;
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public ContactRequestController(
            IContactRequestService contactRequestService,
            IEmployerRepository employerRepository,
            IJobSeekerRepository jobSeekerRepository)
        {
            _contactRequestService = contactRequestService;
            _employerRepository = employerRepository;
            _jobSeekerRepository = jobSeekerRepository;
        }

        // POST: api/contactrequest (Employer sends contact request)
        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> SendContactRequest([FromBody] CreateContactRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return StatusCode(403, new { message = "Employer profile not found for current user." });
            }

            var (success, message, result) = await _contactRequestService.SendContactRequestAsync(employer.EmployerId, dto);
            if (!success)
            {
                return BadRequest(new { message });
            }

            return CreatedAtAction(nameof(GetEmployerRequests), new { }, result);
        }

        // GET: api/contactrequest/employer (Employer views sent requests)
        [Authorize(Roles = "Employer")]
        [HttpGet("employer")]
        public async Task<IActionResult> GetEmployerRequests()
        {
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

            var requests = await _contactRequestService.GetEmployerRequestsAsync(employer.EmployerId);
            return Ok(requests);
        }

        // GET: api/contactrequest/jobseeker (JobSeeker views received requests)
        [Authorize(Roles = "JobSeeker")]
        [HttpGet("jobseeker")]
        public async Task<IActionResult> GetJobSeekerRequests()
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

            var requests = await _contactRequestService.GetJobSeekerRequestsAsync(jobSeeker.Id);
            return Ok(requests);
        }

        // PUT: api/contactrequest/5/respond (JobSeeker accepts or declines request)
        [Authorize(Roles = "JobSeeker")]
        [HttpPut("{id:int}/respond")]
        public async Task<IActionResult> RespondToContactRequest(int id, [FromBody] RespondContactRequestDto dto)
        {
            if (id <= 0 || dto == null)
            {
                return BadRequest(new { message = "Invalid parameters." });
            }

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

            var (success, message, result) = await _contactRequestService.RespondToContactRequestAsync(id, jobSeeker.Id, dto.Response);
            if (!success)
            {
                if (message.Contains("Unauthorized"))
                {
                    return StatusCode(403, new { message });
                }
                return BadRequest(new { message });
            }

            return Ok(new { message, data = result });
        }
    }
}