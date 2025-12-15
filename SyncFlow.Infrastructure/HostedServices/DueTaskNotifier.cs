using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SyncFlow.Application.Common.Notifications;
using SyncFlow.Domain.Enums;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.HostedServices
{
    public class DueTaskNotifier : BackgroundService
    {
        private readonly IServiceProvider _sp;
        public DueTaskNotifier(IServiceProvider sp) => _sp = sp;

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ISyncFlowDbContext>();
                var notify = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var tomorrow = DateTime.UtcNow.Date.AddDays(1);

                try
                {


                    var bizIds = await db.Businesses
                         .Select(b => b.Id)
                         .ToListAsync(ct);

                    foreach (var bizId in bizIds)
                    {
                        var tasks = await db.Tasks.Include(s => s.Status)
                            .Where(t => t.DueDate <= tomorrow &&
                                        t.DeletedAt == null &&
                                        t.Status.Core == CoreStatus.Pending &&
                                        t.AssignedToId != null &&
                                        t.BusinessId == bizId)
                            .Select(t => new { t.Id, t.Title, t.AssignedToId, t.BusinessId })
                            .IgnoreQueryFilters()
                            .ToListAsync(ct);

                        foreach (var g in tasks.GroupBy(t => t.AssignedToId))
                        {
                            var body = $"Tienes {g.Count()} tareas próximas a vencer.";
                            await notify.SendAsync(new NotificationMessage(
                                g.First().BusinessId,
                                new[] { g.Key!.Value },
                                "Recordatorio de tareas",
                                $"<p>{body}</p>"
                            ), ct);
                        }
                    }

                    await Task.Delay(TimeSpan.FromHours(12), ct);
                    //await Task.Delay(TimeSpan.FromMinutes(1), ct);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

}
