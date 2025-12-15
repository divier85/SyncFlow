using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SyncFlow.Application.Common.Notifications;
using SyncFlow.Application.Interfaces;


//using SyncFlow.Application.Common.Interfaces;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Application.Interfaces.Services.Identity;
using SyncFlow.Infrastructure.Email;
using SyncFlow.Infrastructure.HostedServices;
using SyncFlow.Infrastructure.Services;
using SyncFlow.Infrastructure.Services.Identity;
using SyncFlow.Infrastructure.Services.Notifications;
using SyncFlow.Persistence.Common.Interfaces;
using SyncFlow.Persistence.Contexts;
//using SyncFlow.Persistence.Repositories;

namespace SyncFlow.Infrastructure.Extensions;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrar el DbContext
        services.AddDbContext<SyncFlowDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registrar repositorios genéricos
        //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped(typeof(ISyncFlowDbContext), typeof(SyncFlowDbContext));

        // Registrar servicios
        services.AddScoped(typeof(IProjectService), typeof(ProjectService));
        services.AddScoped(typeof(IProcessService), typeof(ProcessService));
        services.AddScoped(typeof(IPhaseService), typeof(PhaseService));
        services.AddScoped(typeof(ITaskService), typeof(TaskService));
        services.AddScoped(typeof(ITaskFeedbackService), typeof(TaskFeedbackService));
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IProcessTemplateService, ProcessTemplateService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IBusinessService, BusinessService>();

        services.AddSingleton<ISendGridClient>(
    new SendGridClient(configuration["SendGrid:ApiKey"]));

        services.AddScoped<SignalRNotificationService>();
        services.AddScoped<EmailNotificationService>();
        services.AddScoped<INotificationService, NotificationFacade>();
        services.AddHostedService<DueTaskNotifier>();

        if (configuration.GetValue<string>("Redis") is { Length: > 0 } redisConn)
            services.AddStackExchangeRedisCache(o => o.Configuration = redisConn);
        else
            services.AddDistributedMemoryCache();

        services.AddScoped<IDashboardService, DashboardService>();
        services.Decorate<IDashboardService, CachedDashboardService>();   // Scrutor

     

        return services;
    }
}
