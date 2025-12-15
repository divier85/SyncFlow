using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SyncFlow.Application.Common.Notifications;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;      // ISyncFlowDbContext

namespace SyncFlow.Infrastructure.Services.Notifications;

public interface IEmailNotificationService
{
    Task SendEmailBatchAsync(NotificationMessage msg,
                             CancellationToken ct = default);
}

public class EmailNotificationService : IEmailNotificationService
{
    private readonly ISendGridClient _sg;
    private readonly IConfiguration _cfg;
    private readonly ISyncFlowDbContext _db;   // ← usamos tu DbContext

    public EmailNotificationService(ISendGridClient sg,
                                    IConfiguration cfg,
                                    ISyncFlowDbContext db)
    {
        _sg = sg;
        _cfg = cfg;
        _db = db;
    }

    public async Task SendEmailBatchAsync(NotificationMessage msg,
                                          CancellationToken ct = default)
    {
        var from = new EmailAddress(
            _cfg["SendGrid:FromEmail"],
            _cfg["SendGrid:FromName"] ?? "SyncFlow");

        // Obtén los correos de los destinatarios (usuarios activos, no eliminados)
        var emails = await _db.Users
            .Where(u => msg.UserIds.Contains(u.Id) && u.DeletedAt == null)
            .Select(u => u.Email!)
            .Distinct()
            .ToListAsync(ct);

        foreach (var e in emails)
        {
            var mail = MailHelper.CreateSingleEmail(
                from,
                new EmailAddress(e),
                msg.Subject,
                plainTextContent: null,
                htmlContent: msg.HtmlBody);

            var resp = await _sg.SendEmailAsync(mail, ct);

            if ((int)resp.StatusCode >= 400)
            {
                var body = await resp.Body.ReadAsStringAsync(ct);
                throw new InvalidOperationException(
                    $"SendGrid error {(int)resp.StatusCode}: {body}");
            }
        }
    }
}
