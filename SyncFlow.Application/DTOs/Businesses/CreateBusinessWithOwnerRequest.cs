namespace SyncFlow.Application.DTOs.Businesses
{
    public class CreateBusinessWithOwnerRequest
    {
        public string Name { get; set; }
        public string LegalName { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;

        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerPhoneNumber { get; set; }
        public string OwnerPassword { get; set; }
    }
}
