using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _auditService.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("{entityId:guid}")]
        public async Task<IActionResult> GetByEntity(Guid entityId)
        {
            var logs = await _auditService.GetByEntityIdAsync(entityId);
            return Ok(logs);
        }
    }

}
