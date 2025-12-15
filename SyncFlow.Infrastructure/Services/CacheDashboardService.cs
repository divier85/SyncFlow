using Microsoft.Extensions.Caching.Distributed;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.Dashboard;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services
{
    public class CachedDashboardService : IDashboardService
    {
        private readonly IDashboardService _inner;
        private readonly IDistributedCache _cache;
        private readonly IBusinessContext _biz;

        public CachedDashboardService(IDashboardService inner,
                                      IDistributedCache cache,
                                      IBusinessContext biz)
            => (_inner, _cache, _biz) = (inner, cache, biz);

        public async Task<DashboardSummary> GetSummaryAsync(CancellationToken ct = default)
        {
            var key = $"dash:{_biz.BusinessId}";
            var cached = await _cache.GetStringAsync(key, ct);
            if (cached != null)
                return JsonSerializer.Deserialize<DashboardSummary>(cached)!;

            var summary = await _inner.GetSummaryAsync(ct);
            var opts = new DistributedCacheEntryOptions
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(summary), opts, ct);
            return summary;
        }
    }

}
