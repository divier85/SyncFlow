using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities
{
    public class PhaseTemplate : BaseEntity, IMultiTenantEntity
    {
        public Guid ProcessTemplateId { get; set; }
        public ProcessTemplate ProcessTemplate { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; }
        public int Order { get; set; }
        public Guid BusinessId { get; set; }

        public List<TaskTemplate> Tasks { get; set; } = new();
    }

}
