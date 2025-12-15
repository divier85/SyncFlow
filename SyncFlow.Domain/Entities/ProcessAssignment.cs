using SyncFlow.Domain.Common;
using System;

namespace SyncFlow.Domain.Entities
{
    public class ProcessAssignment : BaseEntity, IMultiTenantEntity
    {
        public Guid BusinessId { get; set; }
        public Guid ProcessId { get; set; }
        public Process Process { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
