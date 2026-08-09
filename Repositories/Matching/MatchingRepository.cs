using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.Matching
{
    public class MatchingRepository : IMatchingRepository
    {
        public Task<MatchingInputDto?> GetMatchingInputAsync(
            int jobSeekerId,
            int vacancyId)
        {
            throw new NotImplementedException(
                "Database implementation will be added after the shared database entities are finalized.");
        }

        public Task<List<MatchingInputDto>> GetApplicantMatchingInputsAsync(
            int vacancyId)
        {
            throw new NotImplementedException(
                "Database implementation will be added after the shared database entities are finalized.");
        }
    }
}