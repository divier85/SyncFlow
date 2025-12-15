using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Hubs;

/// <summary>
/// Hub que envía notificaciones en tiempo real.
/// Vive en Infrastructure; la API sólo lo mapea.
/// </summary>
[Authorize]      // exige JWT
public class NotificationHub : Hub
{
    public override Task OnConnectedAsync()
    {
        // Agrupamos las conexiones por usuario: "u-{userId}"
        var group = $"u-{Context.UserIdentifier}";
        return Groups.AddToGroupAsync(Context.ConnectionId, group);
    }
}
