using Moq;
using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Services.Implementations;

namespace SmartRecruitmentMatchingPlatform.Tests
{
    public class AdminServiceTests
    {
        [Fact]
        public async Task GetDashboardSummaryAsync_ReturnsCorrectDashboardCounts()
        {
            // Arrange
            var repositoryMock = new Mock<IAdminRepository>();

            repositoryMock
                .Setup(r => r.GetTotalUsersAsync())
                .ReturnsAsync(10);

            repositoryMock
                .Setup(r => r.GetTotalJobSeekersAsync())
                .ReturnsAsync(5);

            repositoryMock
                .Setup(r => r.GetTotalEmployersAsync())
                .ReturnsAsync(3);

            repositoryMock
                .Setup(r => r.GetTotalVacanciesAsync())
                .ReturnsAsync(8);

            repositoryMock
                .Setup(r => r.GetTotalApplicationsAsync())
                .ReturnsAsync(20);

            var service = new AdminService(repositoryMock.Object);

            // Act
            var result = await service.GetDashboardSummaryAsync();

            // Assert
            Assert.NotNull(result);

            Assert.Equal(10, result.TotalUsers);
            Assert.Equal(5, result.TotalJobSeekers);
            Assert.Equal(3, result.TotalEmployers);
            Assert.Equal(8, result.TotalVacancies);
            Assert.Equal(20, result.TotalApplications);
        }
    }
}