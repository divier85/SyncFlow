using Microsoft.AspNetCore.Http;
using SyncFlow.Application.Common.Tenant;
using System;
using System.Security.Claims;

namespace SyncFlow.Infrastructure.Common.Tenant;

public class BusinessContext : IBusinessContext
{
    private readonly IHttpContextAccessor _http;

    public BusinessContext(IHttpContextAccessor http) => _http = http;

    public Guid BusinessId
    {
        get
        {
            var ctx = _http.HttpContext;
            if (ctx is null)                      // background thread
                return Guid.Empty;

            var claim = ctx.User?.FindFirst("business_id")?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }
}
