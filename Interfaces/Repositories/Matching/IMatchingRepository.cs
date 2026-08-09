using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Matching
{
    public interface IMatchingRepository
    {
        Task<MatchingInputDto?> GetMatchingInputAsync(
            int jobSeekerId,
            int vacancyId);

        Task<List<MatchingInputDto>> GetApplicantMatchingInputsAsync(
            int vacancyId);
    }
}