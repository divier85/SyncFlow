using Microsoft.AspNetCore.SignalR;
using SyncFlow.Application.Common.Notifications;
using SyncFlow.Infrastructure.Hubs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services.Notifications;

/// <summary>
/// Envía el mensaje a cada usuario a través del hub SignalR.
/// </summary>
public class SignalRNotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotificationService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    /// <remarks>
    /// No implementa INotificationService directamente: se invoca
    /// desde NotificationFacade. De este modo evitamos exponer
    /// SignalR en la capa Application.
    /// </remarks>
    public async Task SendAsync(NotificationMessage msg, CancellationToken ct = default)
    {
        // Cada usuario se suscribió en el Hub al grupo "u-{userId}"
        foreach (var userId in msg.UserIds.Distinct())
        {
            await _hub.Clients
                      .Group($"u-{userId}")
                      .SendAsync("notify",
                                 new
                                 {
                                     subject = msg.Subject,
                                     html = msg.HtmlBody
                                 }, ct);
        }
    }
}
