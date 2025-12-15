using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Notes;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;

        public NoteController(INoteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(Guid taskId, [FromQuery] NoteFilter filter)
        {
            var result = await _service.GetByTaskIdAsync(filter);
            return Ok(result);
        }
    }

}
