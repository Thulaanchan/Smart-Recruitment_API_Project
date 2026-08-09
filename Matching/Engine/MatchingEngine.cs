using SmartRecruitmentMatchingPlatform.API.Matching.Scoring;
using SmartRecruitmentMatchingPlatform.API.Matching.SkillGap;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching;

namespace SmartRecruitmentMatchingPlatform.API.Matching.Engine
{
    public class MatchingEngine
    {
        private readonly SkillScoreCalculator _skillScoreCalculator;
        private readonly ExperienceScoreCalculator _experienceScoreCalculator;
        private readonly EducationScoreCalculator _educationScoreCalculator;
        private readonly LocationScoreCalculator _locationScoreCalculator;
        private readonly MatchScoreCalculator _matchScoreCalculator;
        private readonly SkillGapCalculator _skillGapCalculator;

        public MatchingEngine()
        {
            _skillScoreCalculator = new SkillScoreCalculator();
            _experienceScoreCalculator = new ExperienceScoreCalculator();
            _educationScoreCalculator = new EducationScoreCalculator();
            _locationScoreCalculator = new LocationScoreCalculator();
            _matchScoreCalculator = new MatchScoreCalculator();
            _skillGapCalculator = new SkillGapCalculator();
        }

        public MatchResultDto CalculateMatch(
            int jobSeekerId,
            int vacancyId,
            IEnumerable<string> jobSeekerSkills,
            IEnumerable<string> requiredSkills,
            double jobSeekerYearsOfExperience,
            double requiredYearsOfExperience,
            int jobSeekerEducationLevel,
            int requiredEducationLevel,
            string? jobSeekerLocation,
            string? vacancyLocation)
        {
            double skillScore =
                _skillScoreCalculator.CalculateSkillScore(
                    jobSeekerSkills,
                    requiredSkills);

            double experienceScore =
                _experienceScoreCalculator.CalculateExperienceScore(
                    jobSeekerYearsOfExperience,
                    requiredYearsOfExperience);

            double educationScore =
                _educationScoreCalculator.CalculateEducationScore(
                    jobSeekerEducationLevel,
                    requiredEducationLevel);

            double locationScore =
                _locationScoreCalculator.CalculateLocationScore(
                    jobSeekerLocation,
                    vacancyLocation);

            double overallScore =
                _matchScoreCalculator.CalculateOverallScore(
                    skillScore,
                    experienceScore,
                    educationScore,
                    locationScore);

            var matchedSkills =
                _skillGapCalculator.GetMatchedSkills(
                    jobSeekerSkills,
                    requiredSkills);

            var missingSkills =
                _skillGapCalculator.GetMissingSkills(
                    jobSeekerSkills,
                    requiredSkills);

            var matchScore = new MatchScoreDto
            {
                JobSeekerId = jobSeekerId,
                VacancyId = vacancyId,
                SkillScore = skillScore,
                ExperienceScore = experienceScore,
                EducationScore = educationScore,
                LocationScore = locationScore,
                OverallScore = overallScore
            };

            var skillGap = new SkillGapDto
            {
                VacancyId = vacancyId,
                MatchedSkills = matchedSkills,
                MissingSkills = missingSkills
            };

            return new MatchResultDto
            {
                MatchScore = matchScore,
                SkillGap = skillGap
            };
        }
    }
}