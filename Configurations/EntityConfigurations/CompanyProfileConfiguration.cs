using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies;

namespace SmartRecruitmentMatchingPlatform.API.Configurations.EntityConfigurations
{
    public class CompanyProfileConfiguration
        : IEntityTypeConfiguration<CompanyProfile>
    {
        public void Configure(EntityTypeBuilder<CompanyProfile> builder)
        {
            builder.ToTable("CompanyProfiles");
        }
    }
}