using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities
{
    public class Role : BaseEntity, IMultiTenantEntity, IRole
    {
        public string Name { get; set; } = string.Empty;

        // Relación con el negocio (multi-tenant)
        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;

        // Relación con usuarios del negocio
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
