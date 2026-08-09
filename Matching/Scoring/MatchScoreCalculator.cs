namespace SmartRecruitmentMatchingPlatform.API.Matching.Scoring
{
    public class MatchScoreCalculator
    {
        public double CalculateOverallScore(
            double skillScore,
            double experienceScore,
            double educationScore,
            double locationScore)
        {
            skillScore = ClampScore(skillScore);
            experienceScore = ClampScore(experienceScore);
            educationScore = ClampScore(educationScore);
            locationScore = ClampScore(locationScore);

            double overallScore =
                (skillScore * MatchingWeights.Skills / 100.0) +
                (experienceScore * MatchingWeights.Experience / 100.0) +
                (educationScore * MatchingWeights.Education / 100.0) +
                (locationScore * MatchingWeights.Location / 100.0);

            return Math.Round(overallScore, 2);
        }

        private static double ClampScore(double score)
        {
            return Math.Clamp(score, 0.0, 100.0);
        }
    }
}