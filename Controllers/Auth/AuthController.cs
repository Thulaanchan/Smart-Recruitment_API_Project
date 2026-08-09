using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.Interfaces.Services;
using SmartRecruitmentMatchingPlatform.Interfaces.Services.Auth;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;
using System.Security.Claims;

namespace SmartRecruitmentMatchingPlatform.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // REGISTER
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto dto)
        {
            var result =
                await _authService.RegisterAsync(dto);

            return Ok(result);
        }

        // LOGIN
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto dto)
        {
            var result =
                await _authService.LoginAsync(dto);

            return Ok(result);
        }

        // REFRESH TOKEN
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequestDto dto)
        {
            var result =
                await _authService.RefreshTokenAsync(dto);

            return Ok(result);
        }

        // CHANGE PASSWORD
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto dto)
        {
            var userIdValue =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(
                    userIdValue,
                    out int userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user."
                });
            }

            var result =
                await _authService.ChangePasswordAsync(
                    userId,
                    dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message =
                    "Password changed successfully."
            });
        }

        // LOGOUT
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdValue =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(
                    userIdValue,
                    out int userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user."
                });
            }

            await _authService.LogoutAsync(userId);

            return Ok(new
            {
                message = "Logout successful."
            });
        }
    }
}