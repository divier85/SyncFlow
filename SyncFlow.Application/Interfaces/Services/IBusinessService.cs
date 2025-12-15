using SyncFlow.Application.DTOs.Businesses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface IBusinessService
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(Guid id);
        Task<Business> CreateAsync(Business business, CancellationToken cancellationToken);
       Task<Business> CreateBusinessWithOwnerAsync(CreateBusinessWithOwnerRequest request, CancellationToken cancellationToken);
        Task<Business> UpdateAsync(Business business, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
