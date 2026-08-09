using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Configurations.EntityConfigurations
{
    public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
    {
        public void Configure(EntityTypeBuilder<Employer> builder)
        {
            builder.ToTable("Employers");

            builder.HasKey(e => e.EmployerId);

            builder.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.CompanyDescription)
                .HasMaxLength(1000);

            builder.Property(e => e.Location)
                .HasMaxLength(200);

            builder.Property(e => e.Website)
                .HasMaxLength(500);

            builder.HasIndex(e => e.UserId)
                .IsUnique();
        }
    }
}