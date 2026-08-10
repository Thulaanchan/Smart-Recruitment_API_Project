using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Notifications;

namespace SmartRecruitmentMatchingPlatform.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);

        Task<NotificationDto?> GetNotificationByIdAsync(int id);

        Task CreateNotificationAsync(
            int userId,
            string title,
            string message);

        Task<bool> MarkAsReadAsync(int id);

        Task<int> GetUnreadCountAsync(int userId);
    }
}