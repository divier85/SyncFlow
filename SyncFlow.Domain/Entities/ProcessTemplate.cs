using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities
{
    public class ProcessTemplate : BaseEntity, IMultiTenantEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int Version { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public bool IsDraft { get; set; } = true;

        public List<PhaseTemplate> Phases { get; set; } = new();
        public Guid BusinessId { get; set; }
    }

}
