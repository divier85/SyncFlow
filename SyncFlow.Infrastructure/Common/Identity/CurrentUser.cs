using Microsoft.AspNetCore.Http;
using SyncFlow.Application.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SyncFlow.Infrastructure.Common.Identity;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http) => _http = http;

    private ClaimsPrincipal Principal =>
        _http.HttpContext?.User;
    //?? throw new InvalidOperationException("No HttpContext or user principal.");

    public Guid UserId
    {
        get
        {
            if (Principal == null)
                return Guid.Empty;

            var userId = Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }
    }

    public Guid BusinessId
    {
        get
        {
            if (Principal == null)
                return Guid.Empty;

            var claim = Principal.FindFirstValue("business_id");
            return claim != null ? Guid.Parse(claim) : Guid.Empty;
        }
    }

    public bool IsInRole(string role) => Principal.IsInRole(role);

    public IReadOnlyCollection<string> Roles =>
        Principal.Claims
                 .Where(c => c.Type == ClaimTypes.Role)
                 .Select(c => c.Value)
                 .ToArray();
}
