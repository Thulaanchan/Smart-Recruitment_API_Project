using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Skills;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Skills;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Skills
{
    [ApiController]
    [Route("api/skills")]
    [Authorize]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        // GET: api/skills
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillService.GetAllAsync();
            return Ok(skills);
        }

        // GET: api/skills/{skillId}
        [HttpGet("{skillId:int}")]
        public async Task<IActionResult> GetById(int skillId)
        {
            if (skillId <= 0)
            {
                return BadRequest(new { message = "Invalid skill ID." });
            }

            var skill = await _skillService.GetByIdAsync(skillId);
            if (skill == null)
            {
                return NotFound(new { message = "Skill not found." });
            }

            return Ok(skill);
        }

        // POST: api/skills
        [HttpPost]
        [Authorize(Roles = "Employer,Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdSkill = await _skillService.CreateAsync(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { skillId = createdSkill.SkillId },
                    createdSkill);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
