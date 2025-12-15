using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Notes;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Infrastructure.Common;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services
{
    public class NoteService : INoteService
    {
        private readonly ISyncFlowDbContext _context;

        public NoteService(ISyncFlowDbContext context)
        {
            _context = context;
        }

        public async Task<NoteResponse> CreateAsync(CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var note = new Note
            {
                Content = request.Content,
                TaskId = request.TaskId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notes.AddAsync(note, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new NoteResponse
            {
                Id = note.Id,
                Content = note.Content,
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<PagedResult<NoteResponse>> GetByTaskIdAsync(NoteFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException($"filter {nameof(filter)} is required");

            var query = _context.Notes
                .Where(n => n.TaskId == filter.taskId)
                .Select(n => new NoteResponse
                {
                    Id = n.Id,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt
                });


                return await query.ToPagedResultAsync(filter);
        }
    }

}
