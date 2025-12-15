using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.AuditLogs;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace SyncFlow.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly ISyncFlowDbContext _dbContext;
        private readonly IBusinessContext _biz;

        public AuditService(ISyncFlowDbContext dbContext, IBusinessContext biz)
        {
            _dbContext = dbContext;
            _biz = biz;
        }

        public async Task RegisterAsync(AuditLogDto entry, CancellationToken ct)
        {
            var log = new AuditLogEntry
            {
                Action = entry.Action,
                EntityId = entry.EntityId,
                EntityType = entry.EntityType,
                PerformedBy = entry.PerformedBy,
                PerformedAt = entry.PerformedAt,
                BusinessId = _biz.BusinessId,
                Details = entry.Details
            };

            _dbContext.AuditLogEntries.Add(log);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<List<AuditLogDto>> GetAllAsync()
        {
            var logs = await _dbContext.AuditLogEntries
                .OrderByDescending(a => a.PerformedAt)
                .ToListAsync();

            return logs.Select(entry => new AuditLogDto
            {
                Action = entry.Action,
                EntityId = entry.EntityId,
                EntityType = entry.EntityType,
                PerformedBy = entry.PerformedBy,
                PerformedAt = entry.PerformedAt,
                Details = entry.Details
            }).ToList();
        }

        public async Task<List<AuditLogDto>> GetByEntityIdAsync(Guid entityId)
        {
            var logs = await _dbContext.AuditLogEntries
                .Where(a => a.EntityId == entityId)
                .OrderByDescending(a => a.PerformedAt)
                .ToListAsync();

            return logs.Select(entry => new AuditLogDto
            {
                Action = entry.Action,
                EntityId = entry.EntityId,
                EntityType = entry.EntityType,
                PerformedBy = entry.PerformedBy,
                PerformedAt = entry.PerformedAt,
                Details = entry.Details
            }).ToList();
        }
    }

}
