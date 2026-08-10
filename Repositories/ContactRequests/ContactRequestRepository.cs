using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Repositories.ContactRequests
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactRequest?> GetByIdAsync(int contactRequestId)
        {
            return await _context.ContactRequests
                .Include(c => c.Employer)
                .Include(c => c.JobSeeker)
                    .ThenInclude(j => j!.Profile)
                .Include(c => c.Vacancy)
                .FirstOrDefaultAsync(c => c.ContactRequestId == contactRequestId);
        }

        public async Task<IEnumerable<ContactRequest>> GetByEmployerIdAsync(int employerId)
        {
            return await _context.ContactRequests
                .Include(c => c.Employer)
                .Include(c => c.JobSeeker)
                    .ThenInclude(j => j!.Profile)
                .Include(c => c.Vacancy)
                .Where(c => c.EmployerId == employerId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContactRequest>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.ContactRequests
                .Include(c => c.Employer)
                .Include(c => c.JobSeeker)
                    .ThenInclude(j => j!.Profile)
                .Include(c => c.Vacancy)
                .Where(c => c.JobSeekerId == jobSeekerId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(ContactRequest contactRequest)
        {
            await _context.ContactRequests.AddAsync(contactRequest);
        }

        public Task UpdateAsync(ContactRequest contactRequest)
        {
            _context.ContactRequests.Update(contactRequest);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
