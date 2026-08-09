using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.Constants.Roles;
using SmartRecruitmentMatchingPlatform.Interfaces.Services;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Users;

namespace SmartRecruitmentMatchingPlatform.Controllers.Users
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = RoleNames.Administrator)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET ALL USERS
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users =
                await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        // GET USER BY ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user =
                await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }

        // UPDATE USER
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            var result =
                await _userService.UpdateUserAsync(
                    id,
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
                message = "User updated successfully."
            });
        }

        // ACTIVATE USER
        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result =
                await _userService.SetUserStatusAsync(
                    id,
                    true);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "User account activated successfully."
            });
        }

        // DEACTIVATE USER
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result =
                await _userService.SetUserStatusAsync(
                    id,
                    false);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "User account deactivated successfully."
            });
        }

        // DELETE USER
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result =
                await _userService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = "User deleted successfully."
            });
        }
    }
}