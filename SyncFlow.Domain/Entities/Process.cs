using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities;

public class Process : BaseEntity, IMultiTenantEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<Phase> Phases { get; set; } = new List<Phase>();

    public ICollection<ProcessAssignment> Assignments { get; set; } = new List<ProcessAssignment>();
}
