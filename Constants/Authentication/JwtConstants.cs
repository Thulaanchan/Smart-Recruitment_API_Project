namespace SmartRecruitmentMatchingPlatform.Constants.Authentication
{
    public static class JwtConstants
    {
        public const string Bearer = "Bearer";

        public const string AuthorizationHeader = "Authorization";

        public const int AccessTokenExpiryMinutes = 60;

        public const int RefreshTokenExpiryDays = 7;
    }
}