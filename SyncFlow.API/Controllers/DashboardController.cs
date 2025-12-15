using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Dashboard;
using SyncFlow.Application.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _svc;
        public DashboardController(IDashboardService svc) => _svc = svc;

        [HttpGet]
        [Authorize]   // cualquier usuario autenticado
        public async Task<DashboardSummary> Get(CancellationToken ct)
            => await _svc.GetSummaryAsync(ct);
    }

}
