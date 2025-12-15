using SyncFlow.Domain.Common;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities
{
    public class Business : BaseEntity
    {
        public string Name { get; set; }

        public string LegalName { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;

        // Contacto
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Configuración Tenant
        public bool IsActive { get; set; } = true;
        public string SubscriptionPlan { get; set; } = "Free";
        public int StorageLimitMb { get; set; } = 1024;
        public string LogoUrl { get; set; } = string.Empty;
        public string SettingsJson { get; set; } = "{}";


        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }

}
