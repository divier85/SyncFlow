using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.Dashboard;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Enums;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ISyncFlowDbContext _db;
        private readonly IBusinessContext _biz;

        public DashboardService(ISyncFlowDbContext db, IBusinessContext biz)
            => (_db, _biz) = (db, biz);

        public async Task<DashboardSummary> GetSummaryAsync(CancellationToken ct = default)
        {
            var bizId = _biz.BusinessId;
            var today = DateTime.UtcNow.Date;
            var soon = today.AddDays(3);

            // Consulta única con proyecciones ⇒ una ronda a DB
            var q = await _db.Tasks
                .Where(t => t.BusinessId == bizId && t.DeletedAt == null)
                .GroupBy(t => 1)     // truco para agrupar todo en un registro
                .Select(g => new
                {
                    TotalTasks = g.Count(),
                    Pending = g.Count(t => t.Status.Core == CoreStatus.Pending),
                    DueSoon = g.Count(t => t.DueDate <= soon && t.DueDate >= today
                                                  && t.Status.Core == CoreStatus.Pending),
                    Overdue = g.Count(t => t.DueDate < today
                                                  && t.Status.Core == CoreStatus.Pending),
                    CompletedToday = g.Count(t => t.Status.Core == CoreStatus.Completed
                                                  && t.UpdatedAt >= today),
                })
                .FirstOrDefaultAsync(ct);

            q ??= new { TotalTasks = 0, Pending = 0, DueSoon = 0, Overdue = 0, CompletedToday = 0 };

            var loads = await _db.Tasks
                .Where(t => t.BusinessId == bizId
                         && t.Status.Core == CoreStatus.Active
                         && t.AssignedToId != null)
                .GroupBy(t => new { t.AssignedToId, t.AssignedTo.FullName })
                .Select(g => new UserLoad(g.Key.AssignedToId!.Value,
                                          g.Key.FullName,
                                          g.Count()))
                .ToListAsync(ct);

            var projects = await _db.Projects.CountAsync(p => p.BusinessId == bizId && p.DeletedAt == null, ct);
            var processes = await _db.Processes.CountAsync(p => p.BusinessId == bizId && p.DeletedAt == null, ct);

            return new DashboardSummary(projects, processes,
                                        q.TotalTasks, q.Pending, q.DueSoon,
                                        q.Overdue, q.CompletedToday, loads);
        }
    }

}
