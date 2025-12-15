using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Businesses;
using SyncFlow.Application.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessesController : ControllerBase
{
    private readonly IBusinessService _businessService;

    public BusinessesController(IBusinessService businessService)
    {
        _businessService = businessService;
    }

    [HttpGet]
    [Authorize(Roles = "Manager,Admin,Owner")]
    public async Task<IActionResult> GetAll()
    {
        var businesses = await _businessService.GetAllAsync();
        return Ok(businesses);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Manager,Admin,Owner")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var business = await _businessService.GetByIdAsync(id);
        if (business == null) return NotFound();
        return Ok(business);
    }

    [HttpPost("create-with-owner")]
    [AllowAnonymous] // porque aún no hay autenticación
    public async Task<IActionResult> CreateWithOwner([FromBody] CreateBusinessWithOwnerRequest request, CancellationToken cancellationToken)
    {
        var result = await _businessService.CreateBusinessWithOwnerAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create([FromBody] Business business, CancellationToken cancellationToken)
    {
        var created = await _businessService.CreateAsync(business, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Business business, CancellationToken cancellationToken)
    {
        if (id != business.Id) return BadRequest("El ID no coincide");

        var updated = await _businessService.UpdateAsync(business, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _businessService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
