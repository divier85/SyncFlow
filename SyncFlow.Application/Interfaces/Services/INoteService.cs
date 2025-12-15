using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Notes;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface INoteService
    {
        Task<NoteResponse> CreateAsync(CreateNoteRequest request, CancellationToken cancellationToken);
        Task<PagedResult<NoteResponse>> GetByTaskIdAsync(NoteFilter filter);
    }

}
