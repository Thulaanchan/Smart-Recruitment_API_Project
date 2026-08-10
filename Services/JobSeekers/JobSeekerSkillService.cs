using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Skills;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Services.JobSeekers
{
    public class JobSeekerSkillService : IJobSeekerSkillService
    {
        private readonly IJobSeekerSkillRepository _jobSeekerSkillRepository;
        private readonly ISkillRepository _skillRepository;

        public JobSeekerSkillService(
            IJobSeekerSkillRepository jobSeekerSkillRepository,
            ISkillRepository skillRepository)
        {
            _jobSeekerSkillRepository = jobSeekerSkillRepository;
            _skillRepository = skillRepository;
        }

        public async Task<List<JobSeekerSkillDto>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            var jobSeekerSkills = await _jobSeekerSkillRepository.GetByJobSeekerIdAsync(jobSeekerId);
            var allSkills = await _skillRepository.GetAllAsync();
            var skillMap = allSkills.ToDictionary(s => s.SkillId, s => s.SkillName);

            return jobSeekerSkills.Select(js => new JobSeekerSkillDto
            {
                Id = js.Id,
                JobSeekerId = js.JobSeekerId,
                SkillId = js.SkillId,
                SkillName = skillMap.TryGetValue(js.SkillId, out var skillName) ? skillName : string.Empty,
                ProficiencyLevel = js.ProficiencyLevel
            }).ToList();
        }

        public async Task<JobSeekerSkillDto> AddAsync(int jobSeekerId, AddSkillDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Skill payload is required.");
            }

            if (dto.ProficiencyLevel < 1 || dto.ProficiencyLevel > 5)
            {
                throw new ArgumentException("Proficiency level must be between 1 and 5.");
            }

            var skill = await _skillRepository.GetByIdAsync(dto.SkillId);
            if (skill == null)
            {
                throw new ArgumentException($"Skill with ID {dto.SkillId} does not exist.");
            }

            var exists = await _jobSeekerSkillRepository.ExistsAsync(jobSeekerId, dto.SkillId);
            if (exists)
            {
                throw new InvalidOperationException("JobSeeker already has this skill.");
            }

            var entity = new JobSeekerSkill
            {
                JobSeekerId = jobSeekerId,
                SkillId = dto.SkillId,
                ProficiencyLevel = dto.ProficiencyLevel
            };

            var created = await _jobSeekerSkillRepository.CreateAsync(entity);

            return new JobSeekerSkillDto
            {
                Id = created.Id,
                JobSeekerId = created.JobSeekerId,
                SkillId = created.SkillId,
                SkillName = skill.SkillName,
                ProficiencyLevel = created.ProficiencyLevel
            };
        }

        public async Task<JobSeekerSkillDto?> UpdateAsync(
            int jobSeekerId,
            int jobSeekerSkillId,
            AddSkillDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Skill payload is required.");
            }

            if (dto.ProficiencyLevel < 1 || dto.ProficiencyLevel > 5)
            {
                throw new ArgumentException("Proficiency level must be between 1 and 5.");
            }

            var existing = await _jobSeekerSkillRepository.GetByIdAsync(jobSeekerSkillId);
            if (existing == null || existing.JobSeekerId != jobSeekerId)
            {
                return null;
            }

            var skill = await _skillRepository.GetByIdAsync(dto.SkillId);
            if (skill == null)
            {
                throw new ArgumentException($"Skill with ID {dto.SkillId} does not exist.");
            }

            var duplicate = await _jobSeekerSkillRepository.ExistsAsync(jobSeekerId, dto.SkillId, jobSeekerSkillId);
            if (duplicate)
            {
                throw new InvalidOperationException("JobSeeker already has this skill.");
            }

            existing.SkillId = dto.SkillId;
            existing.ProficiencyLevel = dto.ProficiencyLevel;

            await _jobSeekerSkillRepository.UpdateAsync(existing);

            return new JobSeekerSkillDto
            {
                Id = existing.Id,
                JobSeekerId = existing.JobSeekerId,
                SkillId = existing.SkillId,
                SkillName = skill.SkillName,
                ProficiencyLevel = existing.ProficiencyLevel
            };
        }

        public async Task<bool> DeleteAsync(int jobSeekerId, int jobSeekerSkillId)
        {
            var existing = await _jobSeekerSkillRepository.GetByIdAsync(jobSeekerSkillId);
            if (existing == null || existing.JobSeekerId != jobSeekerId)
            {
                return false;
            }

            await _jobSeekerSkillRepository.DeleteAsync(existing);
            return true;
        }
    }
}
