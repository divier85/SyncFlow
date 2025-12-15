using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.TasksFeedback;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskFeedbackController : ControllerBase
    {
        private readonly ITaskFeedbackService _service;

        public TaskFeedbackController(ITaskFeedbackService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskFeedbackRequest request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId, CancellationToken cancellationToken)
        {
            var result = await _service.GetByTaskIdAsync(taskId, cancellationToken);
            return Ok(result);
        }
    }

}
