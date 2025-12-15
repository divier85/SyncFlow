using SyncFlow.Domain.Common;
using SyncFlow.Domain.Enums;
using System;

namespace SyncFlow.Domain.Entities;

public class TaskStatus : BaseEntity, IMultiTenantEntity
{
    public string Name { get; set; } = string.Empty;
    public CoreStatus Core { get; set; }
    public string? UIColor { get; set; }      // #RRGGBB opcional
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;
}