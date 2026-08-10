namespace SmartRecruitmentMatchingPlatform.API.Models.DTOs.Matching
{
    public class MatchingInputDto
    {
        public int JobSeekerId { get; set; }

        public string JobSeekerName { get; set; } = string.Empty;

        public int VacancyId { get; set; }

        public List<string> JobSeekerSkills { get; set; }
            = new List<string>();

        public List<string> RequiredSkills { get; set; }
            = new List<string>();

        public double JobSeekerYearsOfExperience { get; set; }

        public double RequiredYearsOfExperience { get; set; }

        public int JobSeekerEducationLevel { get; set; }

        public int RequiredEducationLevel { get; set; }

        public string? JobSeekerLocation { get; set; }

        public string? VacancyLocation { get; set; }
    }
}