// Infrastructure service registration
using Microsoft.Extensions.DependencyInjection;

namespace SyncFlow.Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
            return services;
        }
    }
}