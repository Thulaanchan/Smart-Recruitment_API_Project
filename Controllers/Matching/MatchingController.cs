using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Matching
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatchingController : ControllerBase
    {
        public MatchingController()
        {
            _matchingService = matchingService;
        }

        // GET: api/matching
        [HttpGet]
        public IActionResult GetMatchingStatus()
        {
            return Ok(new
            {
                message = "Matching endpoint is working."
            });
        }

        // GET: api/matching/vacancy/5
        [HttpGet("vacancy/{vacancyId:int}")]
        public IActionResult GetMatchesByVacancy(int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid vacancy ID."
                });
            }

            return Ok(new
            {
                vacancyId,
                message = "Candidate matching endpoint is working."
            });
        }
    }
}