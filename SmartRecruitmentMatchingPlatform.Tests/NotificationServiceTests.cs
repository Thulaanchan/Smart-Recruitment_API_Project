using Moq;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Notifications;
using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Services.Implementations;

namespace SmartRecruitmentMatchingPlatform.Tests
{
    public class NotificationServiceTests
    {
        [Fact]
        public async Task GetUserNotificationsAsync_ReturnsNotifications()
        {
            // Arrange
            var repositoryMock = new Mock<INotificationRepository>();

            repositoryMock
                .Setup(r => r.GetByUserIdAsync(1))
                .ReturnsAsync(new List<Notification>
                {
                    new Notification
                    {
                        Id = 1,
                        UserId = 1,
                        Title = "Test Title",
                        Message = "Test Message",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    }
                });

            var service = new NotificationService(repositoryMock.Object);

            // Act
            var result = await service.GetUserNotificationsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task MarkAsReadAsync_ReturnsFalse_WhenNotificationNotFound()
        {
            // Arrange
            var repositoryMock = new Mock<INotificationRepository>();

            repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Notification?)null);

            var service = new NotificationService(repositoryMock.Object);

            // Act
            var result = await service.MarkAsReadAsync(99);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUnreadCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var repositoryMock = new Mock<INotificationRepository>();

            repositoryMock
                .Setup(r => r.GetUnreadCountAsync(1))
                .ReturnsAsync(2);

            var service = new NotificationService(repositoryMock.Object);

            // Act
            var result = await service.GetUnreadCountAsync(1);

            // Assert
            Assert.Equal(2, result);
        }
    }
}