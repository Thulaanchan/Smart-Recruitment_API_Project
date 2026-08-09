using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;

namespace SmartRecruitmentMatchingPlatform.API.Configurations.EntityConfigurations
{
    public class CompanyAddressConfiguration
        : IEntityTypeConfiguration<CompanyAddress>
    {
        public void Configure(EntityTypeBuilder<CompanyAddress> builder)
        {
            builder.ToTable("CompanyAddresses");

            builder.HasKey(x => x.CompanyAddressId);

            builder.Property(x => x.AddressLine1)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.AddressLine2)
                .HasMaxLength(250);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.State)
                .HasMaxLength(100);

            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                .HasMaxLength(20);
        }
    }
}