using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        public ApplicationController()
        {
        }

        [HttpGet]
        public IActionResult GetApplications()
        {
            return Ok(new
            {
                message = "Application endpoint is working."
            });
        }
    }
}