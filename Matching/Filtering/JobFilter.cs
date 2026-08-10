using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Matching.Filtering
{
    public class JobFilter
    {
        public List<JobSearchResultDto> FilterJobs(
            IEnumerable<JobSearchResultDto> jobs,
            JobFilterDto filter)
        {
            var query = jobs.AsEnumerable();

            // Filter by keyword in job title
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                string keyword = filter.Keyword.Trim();

                query = query.Where(job =>
                    job.JobTitle.Contains(
                        keyword,
                        StringComparison.OrdinalIgnoreCase));
            }

            // Filter by location
            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                string location = filter.Location.Trim();

                query = query.Where(job =>
                    !string.IsNullOrWhiteSpace(job.Location) &&
                    job.Location.Equals(
                        location,
                        StringComparison.OrdinalIgnoreCase));
            }

            // Filter by required skills
            if (filter.Skills != null && filter.Skills.Count > 0)
            {
                var requestedSkills = filter.Skills
                    .Where(skill => !string.IsNullOrWhiteSpace(skill))
                    .Select(skill => skill.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                query = query.Where(job =>
                {
                    var vacancySkills = job.RequiredSkills
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    return requestedSkills.All(skill =>
                        vacancySkills.Contains(skill));
                });
            }

            // Filter by minimum match score
            if (filter.MinimumMatchScore.HasValue)
            {
                double minimumScore = Math.Clamp(
                    filter.MinimumMatchScore.Value,
                    0,
                    100);

                query = query.Where(job =>
                    job.MatchScore >= minimumScore);
            }

            // Highest matching jobs first
            return query
                .OrderByDescending(job => job.MatchScore)
                .ThenBy(job => job.JobTitle)
                .ToList();
        }
    }
}