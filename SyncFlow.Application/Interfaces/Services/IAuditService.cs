using SyncFlow.Application.DTOs.AuditLogs;
using SyncFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface IAuditService
    {
        Task RegisterAsync(AuditLogDto entry, CancellationToken ct);
        Task<List<AuditLogDto>> GetAllAsync();
        Task<List<AuditLogDto>> GetByEntityIdAsync(Guid entityId);
    }

}
