using SyncFlow.Domain.Common;
using System;

namespace SyncFlow.Domain.Entities;

public class Note: BaseEntity, IMultiTenantEntity
{
    public Guid BusinessId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;
}
