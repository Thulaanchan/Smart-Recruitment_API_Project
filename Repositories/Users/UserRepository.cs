using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.Interfaces.Repositories.Users;
using SmartRecruitmentMatchingPlatform.Models.Entities.Users;

namespace SmartRecruitmentMatchingPlatform.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var normalizedEmail =
                email.Trim().ToLowerInvariant();

            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var normalizedEmail =
                email.Trim().ToLowerInvariant();

            return await _context.Users
                .AnyAsync(
                    u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);

            return user;
        }

        public Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);

            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}