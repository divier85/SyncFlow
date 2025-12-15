using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Projects;
using SyncFlow.Application.DTOs.TaskAssignments;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskAssignmentsController : ControllerBase
    {
        private readonly ITaskAssignmentService _service;

        public TaskAssignmentsController(ITaskAssignmentService taskAssignmentService)
        {
            _service = taskAssignmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] CreateTaskAssignmentRequest request)
        {
            var response = await _service.AssignAsync(request, CancellationToken.None);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskAssignmentRequest request)
        {
            if (id != request.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo de la solicitud.");

            var result = await _service.UpdateAssignAsync(id, request, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId)
        {
            var results = await _service.GetByTaskIdAsync(taskId);
            return Ok(results);
        }
    }
}
