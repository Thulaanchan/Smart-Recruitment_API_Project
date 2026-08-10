namespace SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalJobSeekersAsync();
        Task<int> GetTotalEmployersAsync();
        Task<int> GetTotalVacanciesAsync();
        Task<int> GetTotalApplicationsAsync();
    }
}