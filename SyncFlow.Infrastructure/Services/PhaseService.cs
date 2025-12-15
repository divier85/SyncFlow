using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.DTOs.Phases;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services;

public class PhaseService : IPhaseService
{
    private readonly ISyncFlowDbContext _context;

    public PhaseService(ISyncFlowDbContext context)
    {
        _context = context;
    }

    public async Task<PhaseResponse> CreateAsync(CreatePhaseRequest request, CancellationToken cancellationToken)
    {
        var phase = new Phase
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            ProcessId = request.ProcessId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.Phases.Add(phase);
        await _context.SaveChangesAsync(cancellationToken);

        return new PhaseResponse
        {
            Id = phase.Id,
            Title = phase.Title,
            ProcessId = phase.ProcessId,
            StartDate = phase.StartDate,
            EndDate = phase.EndDate
        };
    }

    public async Task<List<PhaseResponse>> GetAllAsync(
         Guid? processId = null,
         string? title = null,
         DateTime? startDate = null,
         DateTime? endDate = null)
    {
        var query = _context.Phases.AsQueryable();

        if (processId.HasValue)
            query = query.Where(p => p.ProcessId == processId.Value);

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(p => p.Title.ToLower().Contains(title.ToLower()));

        if (startDate.HasValue)
            query = query.Where(p => p.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.EndDate <= endDate.Value);

        var phases = await query.ToListAsync();

        return phases.Select(phase => new PhaseResponse
        {
            Id = phase.Id,
            Title = phase.Title,
            ProcessId = phase.ProcessId,
            StartDate = phase.StartDate,
            EndDate = phase.EndDate
        }).ToList();
    }


    public async Task<PhaseResponse?> GetByIdAsync(Guid id)
    {
        var phase = await _context.Phases.FindAsync(id);

        if (phase == null)
            return null;

        return new PhaseResponse
        {
            Id = phase.Id,
            Title = phase.Title,
            ProcessId = phase.ProcessId,
            StartDate = phase.StartDate,
            EndDate = phase.EndDate
        };
    }

    public async Task<bool> UpdateAsync(Guid id, CreatePhaseRequest request, CancellationToken cancellationToken)
    {
        var phase = await _context.Phases.FindAsync(id);
        if (phase == null) return false;

        phase.Title = request.Title;
        phase.StartDate = request.StartDate;
        phase.EndDate = request.EndDate;
        phase.ProcessId = request.ProcessId;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var phase = await _context.Phases.FindAsync(id);
        if (phase == null) return false;

        _context.Phases.Remove(phase);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
