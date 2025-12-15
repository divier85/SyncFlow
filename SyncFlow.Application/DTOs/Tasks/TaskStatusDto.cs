using SyncFlow.Domain.Enums;
using System;

namespace SyncFlow.Application.DTOs.Tasks
{
    public record TaskStatusDto(Guid Id, string Name, CoreStatus Core, string? UIColor);
    public record CreateTaskStatusDto(string Name, CoreStatus Core, string? UIColor);
}