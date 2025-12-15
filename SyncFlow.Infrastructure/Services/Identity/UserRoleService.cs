using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.Users;
using SyncFlow.Application.Interfaces.Services.Identity;
using SyncFlow.Persistence.Auth;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services.Identity;

public class UserRoleService : IUserRoleService
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly RoleManager<ApplicationRole> _roleMgr;
    private readonly IBusinessContext _biz;
    private readonly ISyncFlowDbContext _context;

    public UserRoleService(UserManager<ApplicationUser> userMgr,
                           RoleManager<ApplicationRole> roleMgr,
                           IBusinessContext biz,
                           ISyncFlowDbContext context)
    {
        _userMgr = userMgr;
        _roleMgr = roleMgr;
        _biz = biz;
        _context = context;
    }

    public async Task<UserRoleResponse> AddRoleAsync(AddRoleToUserRequest dto, CancellationToken ct)
    {
        var user = await _userMgr.Users.Include(s => s.UserRoles).IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == dto.UserId && u.BusinessId == (dto.BusinessId == null ? _biz.BusinessId : (Guid)dto.BusinessId), ct)
            ?? throw new KeyNotFoundException("Usuario no encontrado en este negocio.");

        var role = await _roleMgr.Roles.IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == dto.RoleId && r.BusinessId == (dto.BusinessId == null ? _biz.BusinessId : (Guid)dto.BusinessId), ct)
            ?? throw new KeyNotFoundException("Rol no encontrado en este negocio.");

        //var userRoleExists = await _userMgr.Users.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == dto.UserId && s.BusinessId == (dto.BusinessId == null ? _biz.BusinessId : (Guid)dto.BusinessId)
        //);

        var userRoleExists = (user.UserRoles != null ? user.UserRoles : []).FirstOrDefault(s => s.UserId == dto.UserId && s.RoleId == dto.RoleId);

        if (userRoleExists == null)
        {
            _context.UserRoles.Add(new ApplicationUserRole()
            {
                RoleId = role.Id,
                UserId = dto.UserId,
            });

            await _context.SaveChangesAsync(ct);
        }
        //if (!await _userMgr.IsInRoleAsync(user, role.Name!))
        //    await _userMgr.AddToRoleAsync(user, role.Name!);

        return new UserRoleResponse(user.Id, role.Id, role.Name!);
    }

    public async Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct)
    {
        var role = await _roleMgr.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId && r.BusinessId == _biz.BusinessId, ct);
        if (role is null) return false;

        var user = await _userMgr.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.BusinessId == _biz.BusinessId, ct);
        if (user is null) return false;

        if (await _userMgr.IsInRoleAsync(user, role.Name!))
            await _userMgr.RemoveFromRoleAsync(user, role.Name!);

        return true;
    }

    public async Task<IEnumerable<UserRoleResponse>> GetRolesByUserAsync(Guid userId, CancellationToken ct)
    {
        var user = await _userMgr.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.BusinessId == _biz.BusinessId, ct)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        var roleNames = await _userMgr.GetRolesAsync(user);

        var roles = await _roleMgr.Roles
            .Where(r => r.BusinessId == _biz.BusinessId && roleNames.Contains(r.Name!))
            .ToListAsync(ct);

        return roles.Select(r => new UserRoleResponse(user.Id, r.Id, r.Name!));
    }
}
