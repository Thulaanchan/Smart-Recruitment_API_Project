namespace SmartRecruitmentMatchingPlatform.API.Matching.Scoring
{
    public class EducationScoreCalculator
    {
        public double CalculateEducationScore(
            int jobSeekerEducationLevel,
            int requiredEducationLevel)
        {
            jobSeekerEducationLevel =
                Math.Max(0, jobSeekerEducationLevel);

            requiredEducationLevel =
                Math.Max(0, requiredEducationLevel);

            if (requiredEducationLevel == 0)
                return 100.0;

            if (jobSeekerEducationLevel >= requiredEducationLevel)
                return 100.0;

            double score =
                (double)jobSeekerEducationLevel /
                requiredEducationLevel * 100.0;

            return Math.Round(score, 2);
        }
    }
}