using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Vacancies
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;
        private readonly IEmployerRepository _employerRepository;

        public VacancyController(
            IVacancyService vacancyService,
            IEmployerRepository employerRepository)
        {
            _vacancyService = vacancyService;
            _employerRepository = employerRepository;
        }

        // GET: api/vacancy
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetVacancies()
        {
            var vacancies = await _vacancyService.GetAllVacanciesAsync();
            return Ok(vacancies);
        }

        // GET: api/vacancy/search
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchVacancies(
            [FromQuery] string? keyword,
            [FromQuery] string? location,
            [FromQuery] string? skills)
        {
            var vacancies = await _vacancyService.SearchVacanciesAsync(keyword, location, skills);
            return Ok(vacancies);
        }

        // GET: api/vacancy/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVacancyById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid vacancy ID." });
            }

            var vacancy = await _vacancyService.GetVacancyByIdAsync(id);
            if (vacancy == null)
            {
                return NotFound(new { message = "Vacancy not found." });
            }

            return Ok(vacancy);
        }

        // GET: api/vacancy/employer/{employerId:int}
        [HttpGet("employer/{employerId:int}")]
        public async Task<IActionResult> GetEmployerVacancies(int employerId)
        {
            if (employerId <= 0)
            {
                return BadRequest(new { message = "Invalid employer ID." });
            }

            var vacancies = await _vacancyService.GetEmployerVacanciesAsync(employerId);
            return Ok(vacancies);
        }

        // POST: api/vacancy
        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return ForbiddenOrUnauthorized("Employer profile not found for current user.");
            }

            try
            {
                var created = await _vacancyService.CreateVacancyAsync(employer.EmployerId, model);
                if (created == null)
                {
                    return BadRequest(new { message = "Failed to create vacancy." });
                }

                return CreatedAtAction(nameof(GetVacancyById), new { id = created.VacancyId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/vacancy/5
        [Authorize(Roles = "Employer")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVacancy(
            int id,
            [FromBody] UpdateVacancyDto model)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid vacancy ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return ForbiddenOrUnauthorized("Employer profile not found for current user.");
            }

            try
            {
                var success = await _vacancyService.UpdateVacancyAsync(id, employer.EmployerId, model);
                if (!success)
                {
                    return NotFound(new { message = "Vacancy not found or you are not authorized to update it." });
                }

                return Ok(new { vacancyId = id, message = "Vacancy updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/vacancy/5
        [Authorize(Roles = "Employer")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> CloseVacancy(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid vacancy ID." });
            }

            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return ForbiddenOrUnauthorized("Employer profile not found for current user.");
            }

            var success = await _vacancyService.CloseVacancyAsync(id, employer.EmployerId);
            if (!success)
            {
                return NotFound(new { message = "Vacancy not found or you are not authorized to close it." });
            }

            return Ok(new { vacancyId = id, message = "Vacancy closed successfully." });
        }

        // POST: api/vacancy/5/reopen
        [Authorize(Roles = "Employer")]
        [HttpPost("{id:int}/reopen")]
        public async Task<IActionResult> ReopenVacancy(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid vacancy ID." });
            }

            var employer = await GetCurrentEmployerAsync();
            if (employer == null)
            {
                return ForbiddenOrUnauthorized("Employer profile not found for current user.");
            }

            var success = await _vacancyService.ReopenVacancyAsync(id, employer.EmployerId);
            if (!success)
            {
                return NotFound(new { message = "Vacancy not found or you are not authorized to reopen it." });
            }

            return Ok(new { vacancyId = id, message = "Vacancy reopened successfully." });
        }

        private async Task<SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers.Employer?> GetCurrentEmployerAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return null;
            }

            return await _employerRepository.GetByUserIdAsync(userId);
        }

        private IActionResult ForbiddenOrUnauthorized(string message)
        {
            return StatusCode(403, new { message });
        }
    }
}