using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Matching
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        // GET:
        // api/matching/jobseeker/1/vacancy/5
        [HttpGet("jobseeker/{jobSeekerId:int}/vacancy/{vacancyId:int}")]
        public async Task<IActionResult> GetMatch(
            int jobSeekerId,
            int vacancyId)
        {
            if (jobSeekerId <= 0 || vacancyId <= 0)
            {
                return BadRequest(
                    "JobSeekerId and VacancyId must be greater than zero.");
            }

            var result = await _matchingService.GetMatchAsync(
                jobSeekerId,
                vacancyId);

            if (result == null)
            {
                return NotFound(
                    "Job seeker or vacancy could not be found.");
            }

            return Ok(result);
        }

        // GET:
        // api/matching/vacancy/5/ranked-candidates
        [HttpGet("vacancy/{vacancyId:int}/ranked-candidates")]
        public async Task<IActionResult> GetRankedCandidates(
            int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return BadRequest(
                    "VacancyId must be greater than zero.");
            }

            var candidates =
                await _matchingService.GetRankedCandidatesAsync(vacancyId);

            return Ok(candidates);
        }
    }
}