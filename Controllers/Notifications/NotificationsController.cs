using Microsoft.AspNetCore.Mvc;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Notifications;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Controllers.Notifications
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Get all notifications for a user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications =
                await _notificationService.GetUserNotificationsAsync(userId);

            return Ok(notifications);
        }

        // Get notification by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification =
                await _notificationService.GetNotificationByIdAsync(id);

            if (notification == null)
                return NotFound();

            return Ok(notification);
        }

        // Create a new notification
        [HttpPost]
        public async Task<IActionResult> CreateNotification(
            [FromBody] CreateNotificationDto dto)
        {
            await _notificationService.CreateNotificationAsync(
                dto.UserId,
                dto.Title,
                dto.Message
            );

            return Ok(new
            {
                message = "Notification created successfully."
            });
        }

        // Mark notification as read
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result =
                await _notificationService.MarkAsReadAsync(id);

            if (!result)
                return NotFound();

            return Ok(new
            {
                message = "Notification marked as read."
            });
        }

        // Get unread notification count
        [HttpGet("user/{userId}/unread-count")]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var count =
                await _notificationService.GetUnreadCountAsync(userId);

            return Ok(new
            {
                userId = userId,
                unreadCount = count
            });
        }
    }
}