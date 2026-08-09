using SmartRecruitmentMatchingPlatform.Models.DTOs.Users;

namespace SmartRecruitmentMatchingPlatform.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        Task<UserResponseDto?> GetUserByIdAsync(int userId);

        Task<bool> UpdateUserAsync(
            int userId,
            UpdateUserDto dto);

        Task<bool> SetUserStatusAsync(
            int userId,
            bool isActive);

        Task<bool> DeleteUserAsync(int userId);
    }
}