using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Companies
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public CompanyController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        // GET: api/company
        [HttpGet]
        public async Task<IActionResult> GetCompany()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            var profile = await _employerService.GetByUserIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { message = "Company profile not found." });
            }

            return Ok(profile);
        }

        // PUT: api/company
        [Authorize(Roles = "Employer")]
        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateEmployerProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token." });
            }

            var existingProfile = await _employerService.GetByUserIdAsync(userId);
            if (existingProfile == null)
            {
                return NotFound(new { message = "Company profile not found." });
            }

            var updated = await _employerService.UpdateEmployerAsync(existingProfile.EmployerId, model);
            if (updated == null)
            {
                return BadRequest(new { message = "Failed to update company profile." });
            }

            return Ok(updated);
        }
    }
}