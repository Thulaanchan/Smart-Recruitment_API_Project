namespace SmartRecruitmentMatchingPlatform.API.Matching.Scoring
{
    public class ExperienceScoreCalculator
    {
        public double CalculateExperienceScore(
            double jobSeekerYears,
            double requiredYears)
        {
            jobSeekerYears = Math.Max(0, jobSeekerYears);
            requiredYears = Math.Max(0, requiredYears);

            if (requiredYears == 0)
                return 100.0;

            if (jobSeekerYears >= requiredYears)
                return 100.0;

            double score =
                (jobSeekerYears / requiredYears) * 100.0;

            return Math.Round(score, 2);
        }
    }
}