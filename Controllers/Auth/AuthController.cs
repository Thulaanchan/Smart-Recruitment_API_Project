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

        // ======================================
        // REGISTER
        // ======================================

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto dto)
        {
            try
            {
                var result =
                    await _authService.RegisterAsync(dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ======================================
        // LOGIN
        // ======================================

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto dto)
        {
            try
            {
                var result =
                    await _authService.LoginAsync(dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        // ======================================
        // REFRESH TOKEN
        // ======================================

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequestDto dto)
        {
            try
            {
                var result =
                    await _authService.RefreshTokenAsync(dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        // ======================================
        // CHANGE PASSWORD
        // ======================================

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

            try
            {
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ======================================
        // LOGOUT
        // ======================================

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