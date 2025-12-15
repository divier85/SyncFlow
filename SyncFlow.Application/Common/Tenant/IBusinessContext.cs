using System;

namespace SyncFlow.Application.Common.Tenant
{
    /// <summary>Expone el BusinessId (tenant actual) disponible en la petición.</summary>
    public interface IBusinessContext
    {
        Guid BusinessId { get; }
    }
}
