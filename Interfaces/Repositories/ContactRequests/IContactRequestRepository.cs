using SmartRecruitmentMatchingPlatform.API.Models.Entities.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.ContactRequests
{
    public interface IContactRequestRepository
    {
        Task<ContactRequest?> GetByIdAsync(int contactRequestId);

        Task<IEnumerable<ContactRequest>> GetByEmployerIdAsync(int employerId);

        Task<IEnumerable<ContactRequest>> GetByJobSeekerIdAsync(int jobSeekerId);

        Task AddAsync(ContactRequest contactRequest);

        Task UpdateAsync(ContactRequest contactRequest);

        Task SaveChangesAsync();
    }
}
