using SyncFlow.Application.DTOs.Templates;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface IProcessTemplateService
    {
        Task<List<ProcessTemplateDto>> GetAllAsync(CancellationToken ct);
        Task<List<ProcessTemplateDto>> GetLatestTemplatesAsync(CancellationToken ct);
        Task<List<ProcessTemplateDto>> GetVersionsByNameAsync(string name);
        Task<ProcessTemplateDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Guid> CreateAsync(ProcessTemplateDto dto, CancellationToken ct);
        Task PublishAsync(Guid id, CancellationToken ct);
        Task<ProcessTemplateDto> UpdateAsync(Guid id, ProcessTemplateDto dto, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }

}
