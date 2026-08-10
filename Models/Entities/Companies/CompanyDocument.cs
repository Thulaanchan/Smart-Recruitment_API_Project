namespace SmartRecruitmentMatchingPlatform.API.Models.Entities.Companies
{
    public class CompanyDocument
    {
        public int CompanyDocumentId { get; set; }

        public int CompanyId { get; set; }

        public string DocumentName { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public bool IsVerified { get; set; } = false;

        // Navigation Property
        public Company? Company { get; set; }
    }
}