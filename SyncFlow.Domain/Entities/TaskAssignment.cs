using SyncFlow.Domain.Common;
using System;

namespace SyncFlow.Domain.Entities
{
    public class TaskAssignment : BaseEntity, IMultiTenantEntity
    {
        public Guid BusinessId { get; set; }

        public Guid TaskId { get; set; }
        public Task Task { get; set; } = null!;

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }


}
