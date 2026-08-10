using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartRecruitmentMatchingPlatform.API.Services.Vacancies
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ApplicationDbContext _context;

        public VacancyService(
            IVacancyRepository vacancyRepository,
            ApplicationDbContext context)
        {
            _vacancyRepository = vacancyRepository;
            _context = context;
        }

        public async Task<EmployerVacancyDto?> CreateVacancyAsync(
            int employerId,
            CreateVacancyDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var vacancy = new Vacancy
            {
                EmployerId = employerId,
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Salary = dto.Salary,
                ExperienceRequired = dto.ExperienceRequired,
                RequiredEducationLevel = dto.RequiredEducationLevel,
                CreatedAt = DateTime.UtcNow,
                ClosingDate = dto.ClosingDate,
                IsActive = true
            };

            if (dto.SkillIds != null && dto.SkillIds.Any())
            {
                var distinctSkillIds = dto.SkillIds.Distinct().ToList();
                var existingSkillIds = await _context.Skills
                    .Where(s => distinctSkillIds.Contains(s.SkillId))
                    .Select(s => s.SkillId)
                    .ToListAsync();

                var missingSkillIds = distinctSkillIds.Except(existingSkillIds).ToList();
                if (missingSkillIds.Any())
                {
                    throw new ArgumentException($"Skill IDs do not exist: {string.Join(", ", missingSkillIds)}");
                }

                foreach (var skillId in distinctSkillIds)
                {
                    vacancy.VacancySkills.Add(new VacancySkill
                    {
                        SkillId = skillId
                    });
                }
            }

            await _vacancyRepository.AddAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return await GetVacancyByIdAsync(vacancy.VacancyId);
        }

        public async Task<EmployerVacancyDto?> GetVacancyByIdAsync(
            int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return null;
            }

            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return null;
            }

            return await MapToEmployerVacancyDtoAsync(vacancy);
        }

        public async Task<IEnumerable<EmployerVacancyDto>> GetEmployerVacanciesAsync(
            int employerId)
        {
            var vacancies = await _vacancyRepository.GetByEmployerIdAsync(employerId);
            var result = new List<EmployerVacancyDto>();
            foreach (var vacancy in vacancies)
            {
                result.Add(await MapToEmployerVacancyDtoAsync(vacancy));
            }
            return result;
        }

        public async Task<IEnumerable<EmployerVacancyDto>> GetAllVacanciesAsync()
        {
            var vacancies = await _vacancyRepository.GetAllAsync();
            var result = new List<EmployerVacancyDto>();
            foreach (var vacancy in vacancies)
            {
                result.Add(await MapToEmployerVacancyDtoAsync(vacancy));
            }
            return result;
        }

        public async Task<bool> UpdateVacancyAsync(
            int vacancyId,
            int employerId,
            UpdateVacancyDto dto)
        {
            if (dto == null)
            {
                return false;
            }

            var belongsToEmployer = await _vacancyRepository.BelongsToEmployerAsync(vacancyId, employerId);
            if (!belongsToEmployer)
            {
                return false;
            }

            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return false;
            }

            vacancy.Title = dto.Title;
            vacancy.Description = dto.Description;
            vacancy.Location = dto.Location;
            vacancy.Salary = dto.Salary;
            vacancy.ExperienceRequired = dto.ExperienceRequired;
            vacancy.RequiredEducationLevel = dto.RequiredEducationLevel;
            vacancy.ClosingDate = dto.ClosingDate;
            vacancy.IsActive = dto.IsActive;

            // Update VacancySkills
            if (dto.SkillIds != null)
            {
                var distinctSkillIds = dto.SkillIds.Distinct().ToList();
                if (distinctSkillIds.Any())
                {
                    var existingSkillIds = await _context.Skills
                        .Where(s => distinctSkillIds.Contains(s.SkillId))
                        .Select(s => s.SkillId)
                        .ToListAsync();

                    var missingSkillIds = distinctSkillIds.Except(existingSkillIds).ToList();
                    if (missingSkillIds.Any())
                    {
                        throw new ArgumentException($"Skill IDs do not exist: {string.Join(", ", missingSkillIds)}");
                    }
                }

                vacancy.VacancySkills.Clear();
                foreach (var skillId in distinctSkillIds)
                {
                    vacancy.VacancySkills.Add(new VacancySkill
                    {
                        VacancyId = vacancyId,
                        SkillId = skillId
                    });
                }
            }

            await _vacancyRepository.UpdateAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CloseVacancyAsync(
            int vacancyId,
            int employerId)
        {
            var belongsToEmployer = await _vacancyRepository.BelongsToEmployerAsync(vacancyId, employerId);
            if (!belongsToEmployer)
            {
                return false;
            }

            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return false;
            }

            vacancy.IsActive = false;
            await _vacancyRepository.UpdateAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReopenVacancyAsync(
            int vacancyId,
            int employerId)
        {
            var belongsToEmployer = await _vacancyRepository.BelongsToEmployerAsync(vacancyId, employerId);
            if (!belongsToEmployer)
            {
                return false;
            }

            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return false;
            }

            vacancy.IsActive = true;
            await _vacancyRepository.UpdateAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int vacancyId)
        {
            return await _vacancyRepository.ExistsAsync(vacancyId);
        }

        private async Task<EmployerVacancyDto> MapToEmployerVacancyDtoAsync(Vacancy vacancy)
        {
            var totalApps = await _context.Applications.CountAsync(a => a.VacancyId == vacancy.VacancyId);

            var skillIds = vacancy.VacancySkills?.Select(vs => vs.SkillId).ToList() ?? new List<int>();
            var skillNames = vacancy.VacancySkills?.Where(vs => vs.Skill != null).Select(vs => vs.Skill!.SkillName).ToList() ?? new List<string>();

            return new EmployerVacancyDto
            {
                VacancyId = vacancy.VacancyId,
                EmployerId = vacancy.EmployerId,
                Title = vacancy.Title,
                Description = vacancy.Description ?? string.Empty,
                Location = vacancy.Location ?? string.Empty,
                Salary = vacancy.Salary,
                ExperienceRequired = vacancy.ExperienceRequired,
                RequiredEducationLevel = vacancy.RequiredEducationLevel,
                PostedDate = vacancy.CreatedAt,
                ClosingDate = vacancy.ClosingDate,
                IsActive = vacancy.IsActive,
                TotalApplications = totalApps,
                SkillIds = skillIds,
                SkillNames = skillNames
            };
        }
    }
}