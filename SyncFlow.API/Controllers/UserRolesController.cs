using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.DTOs.Users;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Application.Interfaces.Services.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]      // sólo Admin de cada tenant
public class UserRolesController : ControllerBase
{
    private readonly IUserRoleService _svc;
    public UserRolesController(IUserRoleService svc) => _svc = svc;

    /// POST /api/users/{userId}/roles/{roleId}
    [HttpPost("{userId:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> AddRole(Guid userId, Guid roleId, CancellationToken ct)
    {
        var res = await _svc.AddRoleAsync(new AddRoleToUserRequest(userId, roleId), ct);
        return Ok(res);
    }

    /// DELETE /api/users/{userId}/roles/{roleId}
    [HttpDelete("{userId:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> RemoveRole(Guid userId, Guid roleId, CancellationToken ct)
        => await _svc.RemoveRoleAsync(userId, roleId, ct) ? NoContent() : NotFound();

    /// GET /api/users/{userId}/roles
    [HttpGet("{userId:guid}/roles")]
    public async Task<IActionResult> GetRoles(Guid userId, CancellationToken ct)
        => Ok(await _svc.GetRolesByUserAsync(userId, ct));
}
