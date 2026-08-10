using Moq;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Services.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Enums.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.Tests
{
    public class ContactRequestServiceTests
    {
        [Fact]
        public async Task RespondToContactRequestAsync_UpdatesStatus_WhenValidPendingRequest()
        {
            // Arrange
            var contactRepoMock = new Mock<IContactRequestRepository>();
            var employerRepoMock = new Mock<IEmployerRepository>();
            var jobSeekerRepoMock = new Mock<IJobSeekerRepository>();
            var vacancyRepoMock = new Mock<IVacancyRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            var existing = new ContactRequest
            {
                ContactRequestId = 5,
                EmployerId = 2,
                JobSeekerId = 1,
                Status = ContactRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            contactRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);

            var service = new ContactRequestService(
                contactRepoMock.Object,
                employerRepoMock.Object,
                jobSeekerRepoMock.Object,
                vacancyRepoMock.Object,
                notificationServiceMock.Object);

            // Act
            var (success, message, result) = await service.RespondToContactRequestAsync(5, 1, ContactRequestStatus.Accepted);

            // Assert
            Assert.True(success);
            Assert.Equal(ContactRequestStatus.Accepted, existing.Status);
            Assert.NotNull(existing.RespondedAt);
        }
    }
}
