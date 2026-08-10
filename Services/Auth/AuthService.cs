using Microsoft.AspNetCore.Identity;
using SmartRecruitmentMatchingPlatform.Constants.Authentication;
using SmartRecruitmentMatchingPlatform.Interfaces.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Interfaces.Services;
using SmartRecruitmentMatchingPlatform.Interfaces.Services.Auth;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;
using SmartRecruitmentMatchingPlatform.Models.Entities.Users;
using SmartRecruitmentMatchingPlatform.Models.Enums.Users;

using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IEmployerRepository _employerRepository;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IPasswordHasher<User> passwordHasher,
            IJobSeekerRepository jobSeekerRepository,
            IEmployerRepository employerRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _jobSeekerRepository = jobSeekerRepository;
            _employerRepository = employerRepository;
        }

        // =========================
        // REGISTER
        // =========================
        public async Task<AuthResponseDto> RegisterAsync(
            RegisterRequestDto dto)
        {
            var normalizedEmail =
                dto.Email.Trim().ToLowerInvariant();

            var emailExists =
                await _userRepository.EmailExistsAsync(
                    normalizedEmail);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    AuthMessages.EmailAlreadyExists);
            }

            // Only JobSeeker and Employer can self-register
            if (!Enum.IsDefined(typeof(UserRole), dto.Role))
            {
                throw new InvalidOperationException(
                    "Invalid user role.");
            }

            if (dto.Role == UserRole.Administrator)
            {
                throw new InvalidOperationException(
                    "Administrator cannot self-register.");
            }

            // Manual mapping - No AutoMapper
            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = normalizedEmail,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Hash password before saving
            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Automatically create corresponding JobSeeker or Employer domain profile entity
            if (dto.Role == UserRole.JobSeeker)
            {
                var jobSeeker = new SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.JobSeeker
                {
                    UserId = user.Id,
                    Profile = new SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.JobSeekerProfile
                    {
                        FullName = user.FullName,
                        CreatedAt = DateTime.UtcNow
                    }
                };
                await _jobSeekerRepository.CreateAsync(jobSeeker);
            }
            else if (dto.Role == UserRole.Employer)
            {
                var employer = new SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers.Employer
                {
                    UserId = user.Id,
                    CompanyName = user.FullName
                };
                await _employerRepository.AddAsync(employer);
                await _employerRepository.SaveChangesAsync();
            }

            return await CreateAuthResponseAsync(user);
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<AuthResponseDto> LoginAsync(
            LoginRequestDto dto)
        {
            var normalizedEmail =
                dto.Email.Trim().ToLowerInvariant();

            var user =
                await _userRepository.GetByEmailAsync(
                    normalizedEmail);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.InvalidCredentials);
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.AccountDisabled);
            }

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password);

            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.InvalidCredentials);
            }

            return await CreateAuthResponseAsync(user);
        }

        // =========================
        // REFRESH TOKEN
        // =========================
        public async Task<AuthResponseDto> RefreshTokenAsync(
            RefreshTokenRequestDto dto)
        {
            var storedToken =
                await _refreshTokenRepository.GetByTokenAsync(
                    dto.RefreshToken);

            if (storedToken == null ||
                !storedToken.IsActive)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.InvalidRefreshToken);
            }

            var user =
                await _userRepository.GetByIdAsync(
                    storedToken.UserId);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.UserNotFound);
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.AccountDisabled);
            }

            // Revoke old refresh token
            storedToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(
                storedToken);

            // Create new refresh token
            var newRefreshTokenValue =
                _jwtService.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = newRefreshTokenValue,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt =
                    _jwtService.GetRefreshTokenExpiry()
            };

            await _refreshTokenRepository.AddAsync(
                newRefreshToken);

            await _refreshTokenRepository.SaveChangesAsync();

            // Create new JWT access token
            var accessToken =
                _jwtService.GenerateAccessToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                AccessToken = accessToken,
                RefreshToken = newRefreshTokenValue
            };
        }

        // =========================
        // CHANGE PASSWORD
        // =========================
        public async Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordDto dto)
        {
            var user =
                await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.CurrentPassword);

            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException(
                    AuthMessages.IncorrectCurrentPassword);
            }

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.NewPassword);

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            await _userRepository.SaveChangesAsync();

            // Revoke old sessions after password change
            await _refreshTokenRepository
                .RevokeAllByUserIdAsync(user.Id);

            await _refreshTokenRepository
                .SaveChangesAsync();

            return true;
        }

        // =========================
        // LOGOUT
        // =========================
        public async Task LogoutAsync(int userId)
        {
            await _refreshTokenRepository
                .RevokeAllByUserIdAsync(userId);

            await _refreshTokenRepository
                .SaveChangesAsync();
        }

        // =========================
        // CREATE AUTH RESPONSE
        // =========================
        private async Task<AuthResponseDto>
            CreateAuthResponseAsync(User user)
        {
            var accessToken =
                _jwtService.GenerateAccessToken(user);

            var refreshTokenValue =
                _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt =
                    _jwtService.GetRefreshTokenExpiry()
            };

            await _refreshTokenRepository.AddAsync(
                refreshToken);

            await _refreshTokenRepository
                .SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }
    }
}