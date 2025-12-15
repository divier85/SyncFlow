using SyncFlow.Domain.Common;
using System;

namespace SyncFlow.Domain.Entities
{
    public class AuditLogEntry : BaseEntity, IMultiTenantEntity
    {
        public Guid BusinessId { get; set; }
        public string Action { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public Guid PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string Details { get; set; }
    }

}
