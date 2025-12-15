using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities;

public class Phase : BaseEntity, IMultiTenantEntity
{
    public Guid BusinessId { get; set; }
    public string Title { get; set; } = string.Empty;

    public Guid ProcessId { get; set; }
    public Process Process { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
