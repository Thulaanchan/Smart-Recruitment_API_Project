using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Vacancies
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VacancyController : ControllerBase
    {
        public VacancyController()
        {
        }

        // GET: api/vacancy
        [HttpGet]
        public IActionResult GetVacancies()
        {
            return Ok(new
            {
                message = "Vacancy endpoint is working."
            });
        }

        // GET: api/vacancy/5
        [HttpGet("{id:int}")]
        public IActionResult GetVacancyById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid vacancy ID."
                });
            }

            return Ok(new
            {
                vacancyId = id,
                message = "Vacancy details endpoint is working."
            });
        }

        // POST: api/vacancy
        [HttpPost]
        public IActionResult CreateVacancy(
            [FromBody] CreateVacancyDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new
            {
                message = "Vacancy created successfully.",
                data = model
            });
        }

        // PUT: api/vacancy/5
        [HttpPut("{id:int}")]
        public IActionResult UpdateVacancy(
            int id,
            [FromBody] UpdateVacancyDto model)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid vacancy ID."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new
            {
                vacancyId = id,
                message = "Vacancy updated successfully.",
                data = model
            });
        }

        // DELETE: api/vacancy/5
        [HttpDelete("{id:int}")]
        public IActionResult CloseVacancy(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid vacancy ID."
                });
            }

            return Ok(new
            {
                vacancyId = id,
                message = "Vacancy closed successfully."
            });
        }
    }
}