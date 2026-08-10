using Microsoft.EntityFrameworkCore;

// ======================================
// Authentication / Users
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Users;

// ======================================
// Employer
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

// ======================================
// Vacancy
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Vacancies;

// ======================================
// Applications
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Applications;

// ======================================
// Job Seeker
// Aliases avoid duplicate JobSeeker conflict
// ======================================
using JobSeekerEntity =
    SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.JobSeeker;

using JobSeekerProfileEntity =
    SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.JobSeekerProfile;

using CVEntity =
    SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.CV;

using EducationEntity =
    SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.Education;

using ExperienceEntity =
    SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.Experience;

using JobSeekerSkillEntity =
    SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers.JobSeekerSkill;


namespace SmartRecruitmentMatchingPlatform.API.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // ======================================
        // Authentication / User Module
        // ======================================

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


        // ======================================
        // Job Seeker Module
        // ======================================

        public DbSet<JobSeekerEntity> JobSeekers { get; set; }
            = null!;

        public DbSet<JobSeekerProfileEntity> JobSeekerProfiles
        {
            get;
            set;
        } = null!;

        public DbSet<CVEntity> CVs { get; set; }
            = null!;

        public DbSet<EducationEntity> Educations { get; set; }
            = null!;

        public DbSet<ExperienceEntity> Experiences { get; set; }
            = null!;

        public DbSet<JobSeekerSkillEntity> JobSeekerSkills
        {
            get;
            set;
        } = null!;


        // ======================================
        // Employer Module
        // ======================================

        public DbSet<Employer> Employers { get; set; }
            = null!;


        // ======================================
        // Vacancy Module
        // ======================================

        public DbSet<Vacancy> Vacancies { get; set; }
            = null!;

        public DbSet<VacancySkill> VacancySkills { get; set; }
            = null!;


        // ======================================
        // Application Module
        // ======================================

        public DbSet<Application> Applications { get; set; }
            = null!;


        // ======================================
        // Model Configuration
        // ======================================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ======================================
            // User Configuration
            // ======================================

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // ======================================
            // RefreshToken Configuration
            // ======================================

            modelBuilder.Entity<RefreshToken>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(r => r.Token)
                .IsUnique();


            // ======================================
            // Job Seeker Profile Relationship
            // ======================================

            modelBuilder.Entity<JobSeekerEntity>()
                .HasOne(j => j.Profile)
                .WithOne(p => p.JobSeeker)
                .HasForeignKey<JobSeekerProfileEntity>(
                    p => p.JobSeekerId)
                .OnDelete(DeleteBehavior.Cascade);


            // ======================================
            // Apply all Entity Configurations
            // ======================================

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}