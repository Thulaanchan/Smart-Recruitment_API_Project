using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Applications
{
    public interface IApplicationService
    {
        Task<(bool Success, string Message, Application? Application)> ApplyAsync(
            int jobSeekerId,
            int vacancyId);

        Task<IEnumerable<Application>> GetApplicationsByVacancyAsync(
            int vacancyId,
            int employerId);

        Task<IEnumerable<Application>> GetJobSeekerApplicationsAsync(
            int jobSeekerId);

        Task<Application?> GetApplicationByIdAsync(
            int applicationId,
            int employerId);

        Task<bool> UpdateApplicationStatusAsync(
            int applicationId,
            string status,
            int employerId);
    }
}