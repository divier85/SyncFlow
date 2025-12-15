using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Process;
using SyncFlow.Application.DTOs.ProcessAssignments;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _service;

        public ProcessController(IProcessService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] ProcessFilter filter)
        {
            var result = await _service.GetAllAsync(filter);
            return Ok(result);
        }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProcessRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(request, CancellationToken.None);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProcessRequest request)
        {
            if (id != request.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo de la solicitud.");

            var result = await _service.UpdateAsync(id, request, CancellationToken.None);
            return Ok(result);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id, CancellationToken.None);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignProcess(Guid id, [FromBody] CreateProcessAssignmentRequest request, CancellationToken cancellationToken)
        {
            request.ProcessId = id;
            var result = await _service.AssignProcessAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("assigned/user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
        {
            var processes = await _service.GetProcessesByUserAsync(userId, ct);
            return Ok(processes);
        }

        [HttpGet("assigned/role/{roleId}")]
        public async Task<IActionResult> GetByRole(Guid roleId, CancellationToken ct)
        {
            var processes = await _service.GetProcessesByRoleAsync(roleId, ct);
            return Ok(processes);
        }

        [HttpPut("assignments/{assignmentId}")]
        public async Task<IActionResult> UpdateAssignment(Guid assignmentId, [FromBody] CreateProcessAssignmentRequest request, CancellationToken ct)
        {
            var updated = await _service.UpdateProcessAssignmentAsync(assignmentId, request, ct);
            return Ok(updated);
        }

        [HttpDelete("assignments/{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(Guid assignmentId, CancellationToken ct)
        {
            var success = await _service.RemoveProcessAssignmentAsync(assignmentId, ct);
            return success ? NoContent() : NotFound();
        }

    }
}
