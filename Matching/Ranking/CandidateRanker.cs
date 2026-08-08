using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Matching.Ranking
{
    public class CandidateRanker
    {
        public List<RankedCandidateDto> RankCandidates(
            IEnumerable<RankedCandidateDto> candidates)
        {
            var rankedCandidates = candidates
                .OrderByDescending(c => c.MatchScore)
                .ThenBy(c => c.CandidateName)
                .ToList();

            for (int i = 0; i < rankedCandidates.Count; i++)
            {
                rankedCandidates[i].Rank = i + 1;
            }

            return rankedCandidates;
        }
    }
}