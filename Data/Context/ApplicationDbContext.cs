using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Notifications;

namespace SmartRecruitmentMatchingPlatform.API.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
    }
}