using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Tasks;
using SyncFlow.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TaskFilter filter)
    {
        var tasks = await _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var task = await _taskService.CreateAsync(request, CancellationToken.None);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var task = await _taskService.UpdateAsync(id, request, CancellationToken.None);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _taskService.DeleteAsync(id, CancellationToken.None);
        if (!result) return NotFound();
        return NoContent();
    }
}
