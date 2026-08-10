using Moq;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Skills;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Skills;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Skills;
using SmartRecruitmentMatchingPlatform.API.Services.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Services.Skills;

namespace SmartRecruitmentMatchingPlatform.Tests
{
    public class SkillServiceTests
    {
        [Fact]
        public async Task CreateAsync_ThrowsInvalidOperationException_WhenDuplicateMasterSkillNameExists()
        {
            // Arrange
            var skillRepoMock = new Mock<ISkillRepository>();
            skillRepoMock.Setup(r => r.GetByNameAsync("C#"))
                .ReturnsAsync(new Skill { SkillId = 1, SkillName = "C#" });

            var service = new SkillService(skillRepoMock.Object);
            var dto = new CreateSkillDto { SkillName = "C#" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsArgumentException_WhenSkillIdDoesNotExist()
        {
            // Arrange
            var jsSkillRepoMock = new Mock<IJobSeekerSkillRepository>();
            var skillRepoMock = new Mock<ISkillRepository>();
            skillRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Skill?)null);

            var service = new JobSeekerSkillService(jsSkillRepoMock.Object, skillRepoMock.Object);
            var dto = new AddSkillDto { SkillId = 999, ProficiencyLevel = 3 };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(1, dto));
        }

        [Fact]
        public async Task AddAsync_ThrowsInvalidOperationException_WhenJobSeekerAlreadyHasSkill()
        {
            // Arrange
            var jsSkillRepoMock = new Mock<IJobSeekerSkillRepository>();
            var skillRepoMock = new Mock<ISkillRepository>();

            skillRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Skill { SkillId = 1, SkillName = "C#" });

            jsSkillRepoMock.Setup(r => r.ExistsAsync(10, 1, null))
                .ReturnsAsync(true);

            var service = new JobSeekerSkillService(jsSkillRepoMock.Object, skillRepoMock.Object);
            var dto = new AddSkillDto { SkillId = 1, ProficiencyLevel = 4 };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddAsync(10, dto));
        }
    }
}
