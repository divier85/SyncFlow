namespace SyncFlow.Domain.Common
{
    public interface IRole : IMultiTenantEntity
    {
        string Name { get; set; }
    }
}
