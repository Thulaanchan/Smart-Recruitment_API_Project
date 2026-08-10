using SmartRecruitmentMatchingPlatform.Models.Entities.Users;

namespace SmartRecruitmentMatchingPlatform.Interfaces.Repositories.Users
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);

        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(int userId);

        Task<RefreshToken> AddAsync(RefreshToken refreshToken);

        Task UpdateAsync(RefreshToken refreshToken);

        Task DeleteAsync(RefreshToken refreshToken);

        Task RevokeAllByUserIdAsync(int userId);

        Task<bool> SaveChangesAsync();
    }
}