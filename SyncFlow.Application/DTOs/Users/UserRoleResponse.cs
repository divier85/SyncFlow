using System;

namespace SyncFlow.Application.DTOs.Users;

public record UserRoleResponse(Guid UserId, Guid RoleId, string RoleName);