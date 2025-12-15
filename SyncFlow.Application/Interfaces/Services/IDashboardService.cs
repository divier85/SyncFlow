using SyncFlow.Application.DTOs.Dashboard;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummary> GetSummaryAsync(CancellationToken ct = default);
    }

}
