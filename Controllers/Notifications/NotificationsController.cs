using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Get all notifications for current authenticated user
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var notifications = await _notificationService.GetUserNotificationsAsync(userId.Value);
            return Ok(notifications);
        }

        // Get notification by ID (secured to current user)
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null || notification.UserId != userId.Value)
            {
                return NotFound(new { message = "Notification not found." });
            }

            return Ok(notification);
        }

        // Mark notification as read
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null || notification.UserId != userId.Value)
            {
                return NotFound(new { message = "Notification not found." });
            }

            var result = await _notificationService.MarkAsReadAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Notification not found." });
            }

            return Ok(new { message = "Notification marked as read." });
        }

        // Get unread notification count for current user
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token user." });
            }

            var count = await _notificationService.GetUnreadCountAsync(userId.Value);
            return Ok(new
            {
                userId = userId.Value,
                unreadCount = count
            });
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}