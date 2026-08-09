using SmartRecruitmentMatchingPlatform.Interfaces.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Interfaces.Services;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Users;

namespace SmartRecruitmentMatchingPlatform.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET ALL USERS
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
        }

        // GET USER BY ID
        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        // UPDATE USER
        public async Task<bool> UpdateUserAsync(
            int userId,
            UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(dto.FullName))
            {
                user.FullName = dto.FullName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var normalizedEmail =
                    dto.Email.Trim().ToLowerInvariant();

                if (!string.Equals(
                        user.Email,
                        normalizedEmail,
                        StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists =
                        await _userRepository.EmailExistsAsync(
                            normalizedEmail);

                    if (emailExists)
                    {
                        throw new InvalidOperationException(
                            "Email is already registered.");
                    }

                    user.Email = normalizedEmail;
                }
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return await _userRepository.SaveChangesAsync();
        }

        // ACTIVATE / DEACTIVATE USER
        public async Task<bool> SetUserStatusAsync(
            int userId,
            bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return await _userRepository.SaveChangesAsync();
        }

        // DELETE USER
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            await _userRepository.DeleteAsync(user);

            return await _userRepository.SaveChangesAsync();
        }
    }
}