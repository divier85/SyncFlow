using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.Interfaces.Services.Identity;
using SyncFlow.Persistence.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleMgr;
        private readonly IBusinessContext _biz;

        public RoleService(RoleManager<ApplicationRole> roleMgr, IBusinessContext biz)
        {
            _roleMgr = roleMgr; _biz = biz;
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest dto)
        {
            var role = new ApplicationRole { Name = dto.Name, BusinessId = dto.BusinessId == null ? _biz.BusinessId : (Guid)dto.BusinessId };
            var result = await _roleMgr.CreateAsync(role);
            if (!result.Succeeded) throw new Exception(string.Join(';', result.Errors.Select(e => e.Description)));

            return new RoleResponse(role.Id, role.Name!);
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync()
            => (await _roleMgr.Roles.ToListAsync())
                .Select(r => new RoleResponse(r.Id, r.Name!));

        public async Task<RoleResponse> GetByName(string name, Guid businessId)
        {
            var role = await _roleMgr.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Name == name && s.BusinessId == businessId);

            if (role == null)
            {
                return null;
            }

            return new RoleResponse(role.Id, role.Name!);
        }


        public async Task<RoleResponse?> UpdateAsync(Guid id, UpdateRoleRequest dto)
        {
            var role = await _roleMgr.FindByIdAsync(id.ToString());
            if (role == null) return null;
            role.Name = dto.Name;
            await _roleMgr.UpdateAsync(role);
            return new RoleResponse(role.Id, role.Name!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleMgr.FindByIdAsync(id.ToString());
            if (role == null) return false;
            await _roleMgr.DeleteAsync(role);
            return true;
        }
    }

}
