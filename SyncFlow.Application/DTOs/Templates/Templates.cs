using System;
using System.Collections.Generic;

namespace SyncFlow.Application.DTOs.Templates
{
    public record TaskTemplateDto(Guid Id, string Title, string? Description, int EstimatedHours);
    public record PhaseTemplateDto(Guid Id, string Name, string Description, int Order, List<TaskTemplateDto> Tasks);
    public record ProcessTemplateDto(Guid Id, string Name, string Description, bool IsDraft, List<PhaseTemplateDto> Phases);
}
