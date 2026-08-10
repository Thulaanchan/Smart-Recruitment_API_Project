using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Notifications;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Notifications;
using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<NotificationDto>>
            GetUserNotificationsAsync(int userId)
        {
            var notifications =
                await _repository.GetByUserIdAsync(userId);

            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<NotificationDto?>
            GetNotificationByIdAsync(int id)
        {
            var notification =
                await _repository.GetByIdAsync(id);

            if (notification == null)
                return null;

            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }

        public async Task CreateNotificationAsync(
            int userId,
            string title,
            string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(notification);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification =
                await _repository.GetByIdAsync(id);

            if (notification == null)
                return false;

            notification.IsRead = true;

            await _repository.UpdateAsync(notification);
            await _repository.SaveChangesAsync();

            return true;
        }
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _repository.GetUnreadCountAsync(userId);
        }
    }
}