using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Employers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployerController : ControllerBase
    {
        public EmployerController()
        {
        }

        // GET: api/employer/profile
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                message = "Employer profile endpoint is working."
            });
        }

        // PUT: api/employer/profile
        [HttpPut("profile")]
        public IActionResult UpdateProfile(
            [FromBody] UpdateEmployerProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new
            {
                message = "Employer profile update endpoint is working.",
                data = model
            });
        }
    }
}