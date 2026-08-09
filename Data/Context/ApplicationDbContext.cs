using Microsoft.EntityFrameworkCore;

using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Users;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Applications;

namespace SmartRecruitmentMatchingPlatform.API.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =====================================
        // Authentication / User Module
        // =====================================
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


        // =====================================
        // Employer / Vacancy Module
        // =====================================
        public DbSet<Employer> Employers { get; set; } = null!;

        public DbSet<Vacancy> Vacancies { get; set; } = null!;

        public DbSet<VacancySkill> VacancySkills { get; set; } = null!;

        public DbSet<Application> Applications { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================
            // User configuration
            // =====================================
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // =====================================
            // RefreshToken configuration
            // =====================================
            modelBuilder.Entity<RefreshToken>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(r => r.Token)
                .IsUnique();


            // =====================================
            // Apply IEntityTypeConfiguration classes
            // =====================================
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}