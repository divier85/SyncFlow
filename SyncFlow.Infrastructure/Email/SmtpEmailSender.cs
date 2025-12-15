using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SyncFlow.Application.Common.Email;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _cfg;
    private readonly ISendGridClient _client;
    private readonly EmailAddress _from;


    public SmtpEmailSender(IConfiguration cfg)
    {

        _client = new SendGridClient(cfg["SendGrid:ApiKey"]);
        _from = new EmailAddress(cfg["SendGrid:FromEmail"],
                                   cfg["SendGrid:FromName"]);
    }

    public async Task SendAsync(string to, string subject, string htmlBody,
                                CancellationToken ct = default)
    {
        var msg = MailHelper.CreateSingleEmail(
                from: _from,
                to: new EmailAddress(to),
                subject: subject,
                plainTextContent: null,
                htmlContent: htmlBody);

        var resp = await _client.SendEmailAsync(msg, ct);

        if ((int)resp.StatusCode >= 400)
        {
            var body = await resp.Body.ReadAsStringAsync(ct);
            throw new InvalidOperationException(
                $"SendGrid error {(int)resp.StatusCode}: {body}");
        }
    }
}
