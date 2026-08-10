using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Matching
{
    public class MatchingRepository : IMatchingRepository
    {
        private readonly ApplicationDbContext _context;

        public MatchingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MatchingInputDto?> GetMatchingInputAsync(
            int jobSeekerId,
            int vacancyId)
        {
            var jobSeeker = await _context.JobSeekers
                .Include(j => j.Profile)
                .FirstOrDefaultAsync(j => j.Id == jobSeekerId);

            var vacancy = await _context.Vacancies
                .Include(v => v.VacancySkills)
                    .ThenInclude(vs => vs.Skill)
                .FirstOrDefaultAsync(v => v.VacancyId == vacancyId);

            if (jobSeeker == null || vacancy == null)
            {
                return null;
            }

            // JobSeeker Skills
            var jobSeekerSkills = await _context.JobSeekerSkills
                .Where(js => js.JobSeekerId == jobSeekerId)
                .Join(_context.Skills, js => js.SkillId, s => s.SkillId, (js, s) => s.SkillName)
                .ToListAsync();

            // Vacancy Skills
            var vacancySkills = vacancy.VacancySkills?
                .Where(vs => vs.Skill != null)
                .Select(vs => vs.Skill!.SkillName)
                .ToList() ?? new List<string>();

            // Experience calculation
            var experiences = await _context.Experiences
                .Where(e => e.JobSeekerId == jobSeekerId)
                .ToListAsync();

            double totalExperienceYears = 0;
            foreach (var exp in experiences)
            {
                var start = exp.StartDate ?? DateTime.UtcNow;
                var end = exp.IsCurrentJob ? DateTime.UtcNow : (exp.EndDate ?? DateTime.UtcNow);
                if (end > start)
                {
                    totalExperienceYears += (end - start).TotalDays / 365.25;
                }
            }

            // Education level calculation
            var educations = await _context.Educations
                .Where(e => e.JobSeekerId == jobSeekerId)
                .ToListAsync();

            int educationLevel = 0;
            foreach (var edu in educations)
            {
                var qual = (edu.Qualification ?? string.Empty).ToLowerInvariant();
                if (qual.Contains("phd") || qual.Contains("doctor"))
                    educationLevel = Math.Max(educationLevel, 4);
                else if (qual.Contains("master"))
                    educationLevel = Math.Max(educationLevel, 3);
                else if (qual.Contains("bachelor") || qual.Contains("degree") || qual.Contains("bsc") || qual.Contains("ba"))
                    educationLevel = Math.Max(educationLevel, 2);
                else
                    educationLevel = Math.Max(educationLevel, 1);
            }

            return new MatchingInputDto
            {
                JobSeekerId = jobSeekerId,
                JobSeekerName = jobSeeker.Profile?.FullName ?? $"JobSeeker {jobSeekerId}",
                VacancyId = vacancyId,
                JobSeekerSkills = jobSeekerSkills,
                RequiredSkills = vacancySkills,
                JobSeekerYearsOfExperience = Math.Round(totalExperienceYears, 1),
                RequiredYearsOfExperience = vacancy.ExperienceRequired,
                JobSeekerEducationLevel = educationLevel,
                RequiredEducationLevel = vacancy.RequiredEducationLevel,
                JobSeekerLocation = jobSeeker.Profile?.Location,
                VacancyLocation = vacancy.Location
            };
        }

        public async Task<List<MatchingInputDto>> GetApplicantMatchingInputsAsync(
            int vacancyId)
        {
            var applicantJobSeekerIds = await _context.Applications
                .Where(a => a.VacancyId == vacancyId)
                .Select(a => a.JobSeekerId)
                .Distinct()
                .ToListAsync();

            var list = new List<MatchingInputDto>();
            foreach (var jsId in applicantJobSeekerIds)
            {
                var input = await GetMatchingInputAsync(jsId, vacancyId);
                if (input != null)
                {
                    list.Add(input);
                }
            }

            return list;
        }
    }
}