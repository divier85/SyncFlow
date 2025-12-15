using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services.Identity
{
    public interface IRoleService
    {
        Task<RoleResponse> CreateAsync(CreateRoleRequest dto);
        Task<IEnumerable<RoleResponse>> GetAllAsync();
        Task<RoleResponse> GetByName(string name, Guid businessId);
        Task<RoleResponse> UpdateAsync(Guid id, UpdateRoleRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }

}
