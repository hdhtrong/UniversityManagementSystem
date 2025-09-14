namespace ORDService.API.Extentions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            //var connectionName = env.IsEnvironment("Development") ? "DockerOrdDBConnection" : "LocalOrdDBConnection";
            //services.AddDbContext<ORDDbContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString(connectionName));
            //});
            //services.AddScoped<Func<ORDDbContext>>((provider) => () => provider.GetService<ORDDbContext>());
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            return services;
        }
    }

}
