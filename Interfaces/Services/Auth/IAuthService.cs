using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;

namespace SmartRecruitmentMatchingPlatform.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(
            RegisterRequestDto dto);

        Task<AuthResponseDto> LoginAsync(
            LoginRequestDto dto);

        Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto dto);

        Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordDto dto);

        Task LogoutAsync(int userId);
    }
}