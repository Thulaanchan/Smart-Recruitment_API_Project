using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard =
                await _adminService.GetDashboardSummaryAsync();

            return Ok(dashboard);
        }
    }
}