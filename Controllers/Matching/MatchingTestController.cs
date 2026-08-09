using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Matching.Engine;
using SmartRecruitmentMatchingPlatform.API.Matching.Filtering;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Matching
{
    [ApiController]
    [Route("api/matching-test")]
    public class MatchingTestController : ControllerBase
    {
        private readonly MatchingEngine _matchingEngine;
        private readonly JobFilter _jobFilter;

        public MatchingTestController(
            MatchingEngine matchingEngine,
            JobFilter jobFilter)
        {
            _matchingEngine = matchingEngine;
            _jobFilter = jobFilter;
        }

        [HttpGet("match")]
        public IActionResult TestMatching()
        {
            var result = _matchingEngine.CalculateMatch(
                jobSeekerId: 1,
                vacancyId: 10,
                jobSeekerSkills: new[] { "C#", "SQL", "Git" },
                requiredSkills: new[] { "C#", "SQL", "ASP.NET Core", "Git" },
                jobSeekerYearsOfExperience: 3,
                requiredYearsOfExperience: 4,
                jobSeekerEducationLevel: 3,
                requiredEducationLevel: 3,
                jobSeekerLocation: "Jaffna",
                vacancyLocation: "Jaffna"
            );

            return Ok(result);
        }

        [HttpGet("jobs")]
        public IActionResult TestJobFiltering()
        {
            var jobs = new List<JobSearchResultDto>
            {
                new JobSearchResultDto
                {
                    VacancyId = 1,
                    JobTitle = "Software Developer",
                    Location = "Jaffna",
                    RequiredSkills = new List<string> { "C#", "SQL" },
                    MatchScore = 85
                },

                new JobSearchResultDto
                {
                    VacancyId = 2,
                    JobTitle = "Backend Developer",
                    Location = "Colombo",
                    RequiredSkills = new List<string>
                    {
                        "C#",
                        "ASP.NET Core"
                    },
                    MatchScore = 92
                },

                new JobSearchResultDto
                {
                    VacancyId = 3,
                    JobTitle = "Junior Software Developer",
                    Location = "Jaffna",
                    RequiredSkills = new List<string>
                    {
                        "C#",
                        "SQL",
                        "Git"
                    },
                    MatchScore = 76
                }
            };

            var filter = new JobFilterDto
            {
                Keyword = "Developer",
                Location = "Jaffna",
                Skills = new List<string> { "C#" },
                MinimumMatchScore = 70
            };

            var result = _jobFilter.FilterJobs(jobs, filter);

            return Ok(result);
        }
    }
}