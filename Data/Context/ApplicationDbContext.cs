using Microsoft.EntityFrameworkCore;

// ======================================
// Authentication / Users
// ======================================
using SmartRecruitmentMatchingPlatform.Models.Entities.Users;

// ======================================
// Employer
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

// ======================================
// Vacancy / Application
// Vacancy, VacancySkill and Application
// are in this root namespace
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities;

// ======================================
// Notifications & Contact Requests & Skills
// ======================================
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Notifications;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Skills;

// ======================================
// Job Seeker aliases
// Prevent duplicate JobSeeker ambiguity
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
        // Vacancy & Skills Module
        // ======================================

        public DbSet<Vacancy> Vacancies { get; set; }
            = null!;

        public DbSet<VacancySkill> VacancySkills { get; set; }
            = null!;

        public DbSet<Skill> Skills { get; set; }
            = null!;


        // ======================================
        // Application Module
        // ======================================

        public DbSet<Application> Applications { get; set; }
            = null!;


        // ======================================
        // Notification Module
        // ======================================

        public DbSet<Notification> Notifications { get; set; }
            = null!;


        // ======================================
        // Contact Request Module
        // ======================================

        public DbSet<ContactRequest> ContactRequests { get; set; }
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
            // Refresh Token Configuration
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
            // Application Configuration
            // ======================================

            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.JobSeekerId, a.VacancyId })
                .IsUnique();

            modelBuilder.Entity<Application>()
                .HasOne<JobSeekerEntity>()
                .WithMany()
                .HasForeignKey(a => a.JobSeekerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Vacancy>()
                .WithMany()
                .HasForeignKey(a => a.VacancyId)
                .OnDelete(DeleteBehavior.Cascade);


            // ======================================
            // Notification Configuration
            // ======================================

            modelBuilder.Entity<Notification>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Notification>()
                .Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Notification>()
                .Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.UserId);


            // ======================================
            // Contact Request Configuration
            // ======================================

            modelBuilder.Entity<ContactRequest>(builder =>
            {
                builder.HasKey(c => c.ContactRequestId);

                builder.HasOne(c => c.Employer)
                    .WithMany()
                    .HasForeignKey(c => c.EmployerId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(c => c.JobSeeker)
                    .WithMany()
                    .HasForeignKey(c => c.JobSeekerId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(c => c.Vacancy)
                    .WithMany()
                    .HasForeignKey(c => c.VacancyId)
                    .OnDelete(DeleteBehavior.SetNull);
            });


            // ======================================
            // Apply IEntityTypeConfiguration classes
            // ======================================

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}