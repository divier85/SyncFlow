using System;

namespace SyncFlow.Application.DTOs.Users;

public record AddRoleToUserRequest(Guid UserId, Guid RoleId, Guid? BusinessId = null);