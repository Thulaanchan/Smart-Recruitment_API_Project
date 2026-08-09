using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching
{
    public interface IMatchingService
    {
        Task<MatchResultDto?> GetMatchAsync(
            int jobSeekerId,
            int vacancyId);

        Task<List<RankedCandidateDto>> GetRankedCandidatesAsync(
            int vacancyId);
    }
}