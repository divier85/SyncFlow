using SyncFlow.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services.Identity;

public interface IUserRoleService
{
    Task<UserRoleResponse> AddRoleAsync(AddRoleToUserRequest dto, CancellationToken ct);
    Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<IEnumerable<UserRoleResponse>> GetRolesByUserAsync(Guid userId, CancellationToken ct);
}
