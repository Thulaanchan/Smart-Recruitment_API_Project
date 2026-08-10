using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Matching;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Matching;
using SmartRecruitmentMatchingPlatform.API.Matching.Engine;
using SmartRecruitmentMatchingPlatform.API.Matching.Ranking;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Services.Matching
{
    public class MatchingService : IMatchingService
    {
        private readonly IMatchingRepository _matchingRepository;
        private readonly MatchingEngine _matchingEngine;
        private readonly CandidateRanker _candidateRanker;

        public MatchingService(
            IMatchingRepository matchingRepository,
            MatchingEngine matchingEngine,
            CandidateRanker candidateRanker)
        {
            _matchingRepository = matchingRepository;
            _matchingEngine = matchingEngine;
            _candidateRanker = candidateRanker;
        }

        public async Task<MatchResultDto?> GetMatchAsync(
            int jobSeekerId,
            int vacancyId)
        {
            var input = await _matchingRepository
                .GetMatchingInputAsync(jobSeekerId, vacancyId);

            if (input == null)
                return null;

            return _matchingEngine.CalculateMatch(
                input.JobSeekerId,
                input.VacancyId,
                input.JobSeekerSkills,
                input.RequiredSkills,
                input.JobSeekerYearsOfExperience,
                input.RequiredYearsOfExperience,
                input.JobSeekerEducationLevel,
                input.RequiredEducationLevel,
                input.JobSeekerLocation,
                input.VacancyLocation);
        }

        public async Task<List<RankedCandidateDto>>
            GetRankedCandidatesAsync(int vacancyId)
        {
            var inputs = await _matchingRepository
                .GetApplicantMatchingInputsAsync(vacancyId);

            var candidates = new List<RankedCandidateDto>();

            foreach (var input in inputs)
            {
                var result = _matchingEngine.CalculateMatch(
                    input.JobSeekerId,
                    input.VacancyId,
                    input.JobSeekerSkills,
                    input.RequiredSkills,
                    input.JobSeekerYearsOfExperience,
                    input.RequiredYearsOfExperience,
                    input.JobSeekerEducationLevel,
                    input.RequiredEducationLevel,
                    input.JobSeekerLocation,
                    input.VacancyLocation);

                candidates.Add(new RankedCandidateDto
                {
                    JobSeekerId = input.JobSeekerId,
                    CandidateName = string.IsNullOrWhiteSpace(input.JobSeekerName) ? $"Candidate {input.JobSeekerId}" : input.JobSeekerName,
                    MatchScore = result.MatchScore.OverallScore
                });
            }

            return _candidateRanker.RankCandidates(candidates);
        }
    }
}