
using HRMService.Application.Services.Implementations;
using HRMService.Infrastructure;
using HRMService.Infrastructure.Interfaces;
using HRMService.Infrastructure.Repositories;
using HRMService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMService.API.Extentions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            var connectionName = env.IsEnvironment("Development") ? "DockerHrmDBConnection" : "LocalHrmDBConnection";
            services.AddDbContext<HrmDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionName));
            });
            services.AddScoped<Func<HrmDbContext>>((provider) => () => provider.GetService<HrmDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IHrmDepartmentRepository, HrmDepartmentRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IHrmDepartmentService, HrmDepartmentService>();
            return services;
        }
    }
}
