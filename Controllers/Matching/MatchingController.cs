using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Matching
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IVacancyRepository _vacancyRepository;

        public MatchingController(
            IMatchingService matchingService,
            IJobSeekerRepository jobSeekerRepository,
            IEmployerRepository employerRepository,
            IVacancyRepository vacancyRepository)
        {
            _matchingService = matchingService;
            _jobSeekerRepository = jobSeekerRepository;
            _employerRepository = employerRepository;
            _vacancyRepository = vacancyRepository;
        }

        // GET: api/matching/status
        [HttpGet("status")]
        public IActionResult GetMatchingStatus()
        {
            return Ok(new
            {
                message = "Matching service is operational."
            });
        }

        // GET: api/matching/jobseeker/1/vacancy/5
        [HttpGet("jobseeker/{jobSeekerId:int}/vacancy/{vacancyId:int}")]
        public async Task<IActionResult> GetMatch(
            int jobSeekerId,
            int vacancyId)
        {
            if (jobSeekerId <= 0 || vacancyId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid job seeker ID or vacancy ID."
                });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            if (User.IsInRole("JobSeeker"))
            {
                var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
                if (jobSeeker == null || jobSeeker.Id != jobSeekerId)
                {
                    return Forbid();
                }
            }
            else if (User.IsInRole("Employer"))
            {
                var employer = await _employerRepository.GetByUserIdAsync(userId);
                if (employer == null || !await _vacancyRepository.BelongsToEmployerAsync(vacancyId, employer.EmployerId))
                {
                    return Forbid();
                }
            }

            var result = await _matchingService.GetMatchAsync(
                jobSeekerId,
                vacancyId);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Job seeker or vacancy could not be found."
                });
            }

            return Ok(result);
        }

        // GET: api/matching/vacancy/5/ranked-candidates
        [Authorize(Roles = "Employer,Administrator")]
        [HttpGet("vacancy/{vacancyId:int}/ranked-candidates")]
        public async Task<IActionResult> GetRankedCandidates(
            int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid vacancy ID."
                });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            if (User.IsInRole("Employer"))
            {
                var employer = await _employerRepository.GetByUserIdAsync(userId);
                if (employer == null || !await _vacancyRepository.BelongsToEmployerAsync(vacancyId, employer.EmployerId))
                {
                    return Forbid();
                }
            }

            var candidates = await _matchingService.GetRankedCandidatesAsync(vacancyId);

            return Ok(candidates);
        }
    }
}