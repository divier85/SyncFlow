using SyncFlow.Domain.Common;
using System;

namespace SyncFlow.Domain.Entities;

public class TaskFeedback : BaseEntity, IMultiTenantEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
}
