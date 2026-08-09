using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies;

namespace SmartRecruitmentMatchingPlatform.API.Configurations.EntityConfigurations
{
    public class CompanyDocumentConfiguration
        : IEntityTypeConfiguration<CompanyDocument>
    {
        public void Configure(EntityTypeBuilder<CompanyDocument> builder)
        {
            builder.ToTable("CompanyDocuments");

            builder.HasKey(x => x.CompanyDocumentId);
        }
    }
}