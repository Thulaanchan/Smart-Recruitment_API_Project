namespace SmartRecruitmentMatchingPlatform.API.Matching.Scoring
{
    public static class MatchingWeights
    {
        public const double Skills = 50.0;
        public const double Experience = 25.0;
        public const double Education = 15.0;
        public const double Location = 10.0;

        public const double Total =
            Skills +
            Experience +
            Education +
            Location;
    }
}