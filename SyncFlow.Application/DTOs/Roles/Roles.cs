using System;

public record CreateRoleRequest(string Name, Guid? BusinessId);
public record UpdateRoleRequest(string Name);
public record RoleResponse(Guid Id, string Name);