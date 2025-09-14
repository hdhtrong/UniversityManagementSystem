using Consul;

namespace ORDService.API.Extentions
{
    public static class ConsulExtensions
    {
        public static async Task RegisterWithConsulAsync(this WebApplication app, IConfiguration configuration)
        {
            var consulAddress = configuration["Consul:Address"] ?? "http://localhost:8500";
            var serviceName = configuration["Consul:ServiceName"] ?? "ord-service";
            var servicePort = int.Parse(configuration["Consul:ServicePort"] ?? "7003");
            var serviceHost = configuration["Consul:ServiceHost"] ?? "localhost";

            var serviceId = $"{serviceName}-{Guid.NewGuid()}";

            var registration = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = serviceName,
                Address = serviceHost,
                Port = servicePort,
                Tags = new[] { "ord", "api" },
                Check = new AgentServiceCheck
                {
                    HTTP = $"http://{serviceHost}:{servicePort}/health",
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
                }
            };

            try
            {
                using var consulClient = new ConsulClient(config =>
                {
                    config.Address = new Uri(consulAddress);
                });

                var consulStatus = await consulClient.Status.Leader();
                if (string.IsNullOrEmpty(consulStatus))
                {
                    app.Logger.LogWarning("❌ Consul is not available, skipping registration.");
                    return;
                }

                await consulClient.Agent.ServiceRegister(registration);
                app.Logger.LogInformation($"✅ Registered with Consul: {serviceId}");

                // Deregister on shutdown
                app.Lifetime.ApplicationStopping.Register(() =>
                {
                    app.Logger.LogInformation("🔌 Deregistering from Consul...");
                    try
                    {
                        consulClient.Agent.ServiceDeregister(serviceId).Wait();
                    }
                    catch (Exception ex)
                    {
                        app.Logger.LogError(ex, "Failed to deregister from Consul.");
                    }
                });
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning($"⚠️ Could not connect to Consul at {consulAddress}. Error: {ex.Message}");
            }
        }
    }
}
