using AuthService.Application.Services;
using AuthService.Application.Services.Implementations;
using AuthService.Applications.Services;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Interfaces;
using AuthService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;

namespace AuthService.API.Extentions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            var connectionName = env.IsEnvironment("Development") ? "DockerAuthDBConnection" : "LocalAuthDBConnection";
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionName));
            });
            services.AddScoped<Func<AuthDbContext>>((provider) => () => provider.GetService<AuthDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICredentialRepository, CredentialRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtManagerService, JwtManagerService>();
            services.AddScoped<ICredentialService, CredentialService>();
            return services;
        }
    }

}
