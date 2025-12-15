using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities
{
    public class Project : BaseEntity, IMultiTenantEntity
    {
        public Guid BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Business Business { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<Process> Processes { get; set; } = new List<Process>();

    }
}
