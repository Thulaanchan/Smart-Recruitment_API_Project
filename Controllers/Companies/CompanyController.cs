using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Companies
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        public CompanyController()
        {
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            return Ok(new
            {
                message = "Company endpoint is working."
            });
        }
    }
}