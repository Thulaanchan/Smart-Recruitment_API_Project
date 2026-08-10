using SmartRecruitmentMatchingPlatform.API.Models.DTOs.Admin;
using SmartRecruitmentMatchingPlatform.API.Repositories.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;

namespace SmartRecruitmentMatchingPlatform.API.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<AdminDashboardDto> GetDashboardSummaryAsync()
        {
            return new AdminDashboardDto
            {
                TotalUsers = await _adminRepository.GetTotalUsersAsync(),
                TotalJobSeekers = await _adminRepository.GetTotalJobSeekersAsync(),
                TotalEmployers = await _adminRepository.GetTotalEmployersAsync(),
                TotalVacancies = await _adminRepository.GetTotalVacanciesAsync(),
                TotalApplications = await _adminRepository.GetTotalApplicationsAsync()
            };
        }
    }
}