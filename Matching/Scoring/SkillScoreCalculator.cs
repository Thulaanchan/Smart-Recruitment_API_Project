namespace SmartRecruitmentMatchingPlatform.API.Matching.Scoring
{
    public class SkillScoreCalculator
    {
        public double CalculateSkillScore(
            IEnumerable<string> jobSeekerSkills,
            IEnumerable<string> requiredSkills)
        {
            var seekerSkills = jobSeekerSkills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var vacancySkills = requiredSkills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (vacancySkills.Count == 0)
                return 100.0;

            int matchedCount = vacancySkills
                .Count(skill => seekerSkills.Contains(skill));

            double score =
                (double)matchedCount / vacancySkills.Count * 100.0;

            return Math.Round(score, 2);
        }
    }
}