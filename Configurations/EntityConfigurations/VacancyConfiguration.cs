using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Configurations.EntityConfigurations
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            // Table Name
            builder.ToTable("Vacancies");

            // Primary Key
            builder.HasKey(v => v.VacancyId);

            // Employer Foreign Key
            builder.Property(v => v.EmployerId)
                .IsRequired();

            // Job Title
            builder.Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Job Description
            builder.Property(v => v.Description)
                .HasMaxLength(2000);

            // Location
            builder.Property(v => v.Location)
                .HasMaxLength(200);

            // Created Date
            builder.Property(v => v.CreatedAt)
                .IsRequired();

            // Employer Relationship
            builder.HasOne(v => v.Employer)
                .WithMany()
                .HasForeignKey(v => v.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}