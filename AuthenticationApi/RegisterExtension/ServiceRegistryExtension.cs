using AuthenticationApi.Settings;
using Consul;

namespace AuthenticationApi.RegisterExtension;
public static class ServiceRegistryExtension
{
    //consul service config
    public static IServiceCollection AddConsulSettings(this IServiceCollection services, ServiceSettings serviceSettings)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(c => new ConsulClient(config =>
        {
            config.Address = new Uri(serviceSettings.ServiceDiscoveryAddress); //add discovery address "http://127.0.0.1:8500"
        }));

        return services;
    }


    public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, ServiceSettings serviceSettings)
    {

        IConsulClient consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        ILogger logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("ServiceRegistryExtension");
        IHostApplicationLifetime lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        AgentServiceRegistration registration = new AgentServiceRegistration()
        {
            ID = serviceSettings.ServiceName, //service name
            Name = serviceSettings.ServiceName,
            Address = serviceSettings.ServiceHost,
            Port = serviceSettings.ServicePort
        };

        logger.LogInformation("🖊️ -> Registering with Consul");
        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true); //Deregister service before registering.
        consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true); //Deregister service before registering.

        lifetime.ApplicationStopping.Register(() => {
            logger.LogInformation("❌ -> Deregistering with Consul");
        });

        return app;
    }
}

