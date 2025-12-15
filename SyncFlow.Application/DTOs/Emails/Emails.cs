using System;

namespace SyncFlow.Application.DTOs.Emails
{
    public record ConfirmEmailRequest(Guid UserId, string Token);
    public record ForgotPasswordRequest(string Email);
    public record ResetPasswordRequest(Guid UserId, string Token, string NewPassword);

}
