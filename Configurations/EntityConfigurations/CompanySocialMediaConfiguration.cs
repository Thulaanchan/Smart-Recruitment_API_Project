using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies;

namespace SmartRecruitmentMatchingPlatform.API.Configurations.EntityConfigurations
{
    public class CompanySocialMediaConfiguration
        : IEntityTypeConfiguration<CompanySocialMedia>
    {
        public void Configure(
            EntityTypeBuilder<CompanySocialMedia> builder)
        {
            builder.ToTable("CompanySocialMedias");

            builder.HasKey(x => x.CompanySocialMediaId);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}