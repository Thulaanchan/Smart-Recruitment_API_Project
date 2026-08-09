using SmartRecruitmentMatchingPlatform.Models.Entities.Users;

namespace SmartRecruitmentMatchingPlatform.Interfaces.Services.Auth
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        DateTime GetRefreshTokenExpiry();
    }
}