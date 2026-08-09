namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers
{
    public class JobSeeker
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public JobSeekerProfile? Profile { get; set; }
    }
}