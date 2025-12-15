using SyncFlow.Application.Common.Notifications;
using SyncFlow.Infrastructure.Email;
using SyncFlow.Infrastructure.Services.Notifications;
using System.Threading;
using System.Threading.Tasks;

public class NotificationFacade : INotificationService
{
    private readonly EmailNotificationService _email;
    private readonly SignalRNotificationService _signalr;

    public NotificationFacade(EmailNotificationService e, SignalRNotificationService s)
        => (_email, _signalr) = (e, s);

    public async Task SendAsync(NotificationMessage msg, CancellationToken ct = default)
    {
        await _signalr.SendAsync(msg, ct);
        await _email.SendEmailBatchAsync(msg, ct);   // opcional: flag usuario
    }
}
