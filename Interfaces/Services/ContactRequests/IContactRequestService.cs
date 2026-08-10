using SmartRecruitmentMatchingPlatform.API.Models.DTOs.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Enums.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Services.ContactRequests
{
    public interface IContactRequestService
    {
        Task<(bool Success, string Message, ContactRequestDto? Result)> SendContactRequestAsync(
            int employerId,
            CreateContactRequestDto dto);

        Task<IEnumerable<ContactRequestDto>> GetEmployerRequestsAsync(int employerId);

        Task<IEnumerable<ContactRequestDto>> GetJobSeekerRequestsAsync(int jobSeekerId);

        Task<(bool Success, string Message, ContactRequestDto? Result)> RespondToContactRequestAsync(
            int contactRequestId,
            int jobSeekerId,
            ContactRequestStatus status);
    }
}
