using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Phases;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhasesController : ControllerBase
    {
        private readonly IPhaseService _service;

        public PhasesController(IPhaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePhaseRequest request)
        {
            var result = await _service.CreateAsync(request, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
             [FromQuery] Guid? processId,
             [FromQuery] string? title,
             [FromQuery] DateTime? startDate,
             [FromQuery] DateTime? endDate)
        {
            var phases = await _service.GetAllAsync(processId, title, startDate, endDate);
            return Ok(phases);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreatePhaseRequest request)
        {
            var updated = await _service.UpdateAsync(id, request, CancellationToken.None);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id, CancellationToken.None);
            return deleted ? NoContent() : NotFound();
        }
    }

}
