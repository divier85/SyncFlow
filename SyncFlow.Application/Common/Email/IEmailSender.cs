using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Common.Email;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}
