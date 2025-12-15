using SyncFlow.Domain.Common;
using System;

namespace SyncFlow.Domain.Entities
{
    public class TaskTemplate : BaseEntity, IMultiTenantEntity
    {
        public Guid PhaseTemplateId { get; set; }
        public PhaseTemplate PhaseTemplate { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public int EstimatedHours { get; set; }
        public Guid BusinessId { get; set; }
        public int Order { get; set; }
    }

}
