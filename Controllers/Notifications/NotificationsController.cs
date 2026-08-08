using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications =
                await _notificationService.GetUserNotificationsAsync(userId);

            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification =
                await _notificationService.GetNotificationByIdAsync(id);

            if (notification == null)
                return NotFound();

            return Ok(notification);
        }

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
        [HttpPost("test")]
        public async Task<IActionResult> CreateTestNotification()
        {
            await _notificationService.CreateNotificationAsync(
                1,
                "Application Status Updated",
                "Your application has been shortlisted."
            );

            return Ok(new
            {
                message = "Test notification created successfully."
            });
        }
    }

}