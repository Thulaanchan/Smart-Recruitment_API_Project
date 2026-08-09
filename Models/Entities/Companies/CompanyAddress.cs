namespace SmartRecruitmentMatchingPlatform.API.Models.Entities
{
    public class CompanyAddress
    {
        public int CompanyAddressId { get; set; }

        public int CompanyId { get; set; }

        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = string.Empty;

        public string? State { get; set; }

        public string Country { get; set; } = string.Empty;

        public string? PostalCode { get; set; }
    }
}