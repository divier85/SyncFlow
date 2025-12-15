using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyncFlow.Application.Interfaces.Services.Identity;
using SyncFlow.Persistence.Auth;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _svc;
    public RolesController(IRoleService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleRequest dto)
        => Ok(await _svc.CreateAsync(dto));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _svc.GetAllAsync());

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoleRequest dto)
    {
        var r = await _svc.UpdateAsync(id, dto);
        return r is null ? NotFound() : Ok(r);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await _svc.DeleteAsync(id) ? NoContent() : NotFound();


    [HttpPost("/users/{userId:guid}/roles/{roleId:guid}")]
    public async Task<IActionResult> AddRole(Guid userId, Guid roleId,
    UserManager<ApplicationUser> userMgr, RoleManager<ApplicationRole> roleMgr)
    {
        var user = await userMgr.FindByIdAsync(userId.ToString());
        var role = await roleMgr.FindByIdAsync(roleId.ToString());
        if (user == null || role == null) return NotFound();

        await userMgr.AddToRoleAsync(user, role.Name!);
        return NoContent();
    }

    [HttpDelete("/users/{userId}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRole(Guid userId, Guid roleId,
        UserManager<ApplicationUser> userMgr, RoleManager<ApplicationRole> roleMgr)
    {
        var user = await userMgr.FindByIdAsync(userId.ToString());
        var role = await roleMgr.FindByIdAsync(roleId.ToString());
        if (user == null || role == null) return NotFound();

        await userMgr.RemoveFromRoleAsync(user, role.Name!);
        return NoContent();
    }

}
