using SmartRecruitmentMatchingPlatform.API.Matching.Engine;
using SmartRecruitmentMatchingPlatform.API.Matching.Ranking;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.Tests
{
    public class MatchingEngineTests
    {
        [Fact]
        public void CalculateMatch_ReturnsCorrectWeights()
        {
            // Arrange
            var engine = new MatchingEngine();

            // Perfect match
            var result = engine.CalculateMatch(
                jobSeekerId: 1,
                vacancyId: 10,
                jobSeekerSkills: new[] { "C#", "SQL" },
                requiredSkills: new[] { "C#", "SQL" },
                jobSeekerYearsOfExperience: 5,
                requiredYearsOfExperience: 5,
                jobSeekerEducationLevel: 3,
                requiredEducationLevel: 3,
                jobSeekerLocation: "Colombo",
                vacancyLocation: "Colombo"
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100.0, result.MatchScore.OverallScore, precision: 1);
            Assert.Equal(100.0, result.MatchScore.SkillScore, precision: 1);
            Assert.Equal(100.0, result.MatchScore.ExperienceScore, precision: 1);
            Assert.Equal(100.0, result.MatchScore.EducationScore, precision: 1);
            Assert.Equal(100.0, result.MatchScore.LocationScore, precision: 1);
        }

        [Fact]
        public void CandidateRanker_RanksCandidatesByScoreDescending()
        {
            // Arrange
            var ranker = new CandidateRanker();
            var candidates = new List<RankedCandidateDto>
            {
                new RankedCandidateDto { JobSeekerId = 1, CandidateName = "Alice", MatchScore = 75.0 },
                new RankedCandidateDto { JobSeekerId = 2, CandidateName = "Bob", MatchScore = 95.0 },
                new RankedCandidateDto { JobSeekerId = 3, CandidateName = "Charlie", MatchScore = 85.0 }
            };

            // Act
            var ranked = ranker.RankCandidates(candidates);

            // Assert
            Assert.Equal(3, ranked.Count);
            Assert.Equal("Bob", ranked[0].CandidateName);
            Assert.Equal("Charlie", ranked[1].CandidateName);
            Assert.Equal("Alice", ranked[2].CandidateName);
        }
    }
}
