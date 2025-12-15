using System;

namespace SyncFlow.Domain.Common
{
    /// <summary>Interfaz marcador para entidades multitenant.</summary>
    public interface IMultiTenantEntity
    {
        Guid BusinessId { get; set; }
    }
}
