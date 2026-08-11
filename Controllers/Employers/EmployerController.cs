using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Employers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Employers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        // GET: api/employer/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var profile = await _employerService.GetByUserIdAsync(userId.Value);
            if (profile == null)
            {
                return NotFound(new { message = "Employer profile not found." });
            }

            return Ok(profile);
        }

        // PUT: api/employer/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateEmployerProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var existingProfile = await _employerService.GetByUserIdAsync(userId.Value);
            if (existingProfile == null)
            {
                return NotFound(new { message = "Employer profile not found." });
            }

            var updated = await _employerService.UpdateEmployerAsync(existingProfile.EmployerId, model);
            if (updated == null)
            {
                return BadRequest(new { message = "Failed to update profile." });
            }

            return Ok(updated);
        }

        // GET: api/employer/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var profile = await _employerService.GetByUserIdAsync(userId.Value);
            if (profile == null)
            {
                return NotFound(new { message = "Employer profile not found." });
            }

            var dashboard = await _employerService.GetDashboardAsync(profile.EmployerId);
            return Ok(dashboard);
        }

        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(claim, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}