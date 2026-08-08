namespace SmartRecruitmentMatchingPlatform.API.Matching.SkillGap
{
    public class SkillGapCalculator
    {
        public List<string> GetMatchedSkills(
            IEnumerable<string> jobSeekerSkills,
            IEnumerable<string> requiredSkills)
        {
            var seekerSkills = jobSeekerSkills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return requiredSkills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Where(skill => seekerSkills.Contains(skill))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public List<string> GetMissingSkills(
            IEnumerable<string> jobSeekerSkills,
            IEnumerable<string> requiredSkills)
        {
            var seekerSkills = jobSeekerSkills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return requiredSkills
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Where(skill => !seekerSkills.Contains(skill))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}