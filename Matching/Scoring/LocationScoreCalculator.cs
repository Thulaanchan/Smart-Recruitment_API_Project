namespace SmartRecruitmentMatchingPlatform.API.Matching.Scoring
{
    public class LocationScoreCalculator
    {
        public double CalculateLocationScore(
            string? jobSeekerLocation,
            string? vacancyLocation)
        {
            if (string.IsNullOrWhiteSpace(vacancyLocation))
                return 100.0;

            if (string.IsNullOrWhiteSpace(jobSeekerLocation))
                return 0.0;

            string seekerLocation =
                jobSeekerLocation.Trim();

            string requiredLocation =
                vacancyLocation.Trim();

            return string.Equals(
                seekerLocation,
                requiredLocation,
                StringComparison.OrdinalIgnoreCase)
                ? 100.0
                : 0.0;
        }
    }
}