using Moq;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Applications;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;
using SmartRecruitmentMatchingPlatform.API.Services.Applications;

namespace SmartRecruitmentMatchingPlatform.Tests
{
    public class ApplicationServiceTests
    {
        [Fact]
        public async Task ApplyAsync_Fails_WhenJobSeekerHasAlreadyApplied()
        {
            // Arrange
            var appRepoMock = new Mock<IApplicationRepository>();
            var vacancyRepoMock = new Mock<IVacancyRepository>();

            vacancyRepoMock.Setup(v => v.ExistsAsync(10)).ReturnsAsync(true);
            appRepoMock.Setup(a => a.HasAppliedAsync(1, 10)).ReturnsAsync(true);

            var service = new ApplicationService(appRepoMock.Object, vacancyRepoMock.Object);

            // Act
            var (success, message, app) = await service.ApplyAsync(1, 10);

            // Assert
            Assert.False(success);
            Assert.Contains("already applied", message);
            Assert.Null(app);
        }

        [Fact]
        public async Task ApplyAsync_Succeeds_WhenFirstTimeApplication()
        {
            // Arrange
            var appRepoMock = new Mock<IApplicationRepository>();
            var vacancyRepoMock = new Mock<IVacancyRepository>();

            vacancyRepoMock.Setup(v => v.ExistsAsync(10)).ReturnsAsync(true);
            appRepoMock.Setup(a => a.HasAppliedAsync(1, 10)).ReturnsAsync(false);

            var service = new ApplicationService(appRepoMock.Object, vacancyRepoMock.Object);

            // Act
            var (success, message, app) = await service.ApplyAsync(1, 10);

            // Assert
            Assert.True(success);
            Assert.NotNull(app);
            Assert.Equal(1, app.JobSeekerId);
            Assert.Equal(10, app.VacancyId);
        }
    }
}
