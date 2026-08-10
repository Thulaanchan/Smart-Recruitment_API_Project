using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Admin;

namespace SmartRecruitmentMatchingPlatform.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardSummaryAsync();
    }
}