using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Templates;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessTemplatesController : ControllerBase
{
    private readonly IProcessTemplateService _templateService;

    public ProcessTemplatesController(IProcessTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcessTemplateDto>>> GetAll(CancellationToken ct)
    {
        var result = await _templateService.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestTemplates(CancellationToken ct)
    {
        var templates = await _templateService.GetLatestTemplatesAsync(ct);
        return Ok(templates);
    }

    [HttpGet("{name}/versions")]
    public async Task<IActionResult> GetVersionsByName(string name)
    {
        var versions = await _templateService.GetVersionsByNameAsync(name);
        return Ok(versions);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<ProcessTemplateDto>> Get(Guid id, CancellationToken ct)
    {
        var template = await _templateService.GetByIdAsync(id, ct);
        return template is null ? NotFound() : Ok(template);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(ProcessTemplateDto command, CancellationToken ct)
    {
        var id = await _templateService.CreateAsync(command, ct);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPost("{id}/publish")]
    public async Task<IActionResult> PublishTemplate(Guid id, CancellationToken ct)
    {
        await _templateService.PublishAsync(id, ct);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ProcessTemplateDto command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest();
        await _templateService.UpdateAsync(id, command, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _templateService.DeleteAsync(id, ct);
        return NoContent();
    }
}
