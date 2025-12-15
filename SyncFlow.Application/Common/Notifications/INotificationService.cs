// SyncFlow.Application/Common/Notifications/NotificationMessage.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Common.Notifications;

public record NotificationMessage(
    Guid BusinessId,
    IEnumerable<Guid> UserIds,
    string Subject,
    string HtmlBody);

public interface INotificationService
{
    Task SendAsync(NotificationMessage msg, CancellationToken ct = default);
}
