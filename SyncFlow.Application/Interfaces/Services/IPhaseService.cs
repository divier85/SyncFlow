using SyncFlow.Application.DTOs.Phases;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface IPhaseService
    {
        Task<PhaseResponse> CreateAsync(CreatePhaseRequest request, CancellationToken cancellationToken);
        Task<List<PhaseResponse>> GetAllAsync(Guid? processId = null, string? title = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<PhaseResponse?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, CreatePhaseRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }

}
