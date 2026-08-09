using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.ContactRequests
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactRequestController : ControllerBase
    {
        public ContactRequestController()
        {
        }

        [HttpGet]
        public IActionResult GetContactRequests()
        {
            return Ok(new
            {
                message = "Contact Request endpoint is working."
            });
        }
    }
}