﻿using AuthenticationApi.Settings;
using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace AuthenticationApi.RegisterExtension;
public static class ConsuleRegistration
{
    public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(c => new ConsulClient(consulConfig =>
        {
            var address = configuration["ServiceSettings:ServiceDiscoveryAddress"];
            consulConfig.Address = new Uri(address);
        }));

        return services;
    }

    public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

        var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

        var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

        // Get server ip address
        var features = app.Properties["server.Features"] as FeatureCollection;
        var addresses = features.Get<IServerAddressesFeature>();
        if (addresses.Addresses.Count == 0)
        {
            // Add the default address to the IServerAddressesFeature if it does not exist
            addresses.Addresses.Add("http://localhost:5001");
        }
        var address = addresses.Addresses.First();

        // Register service with consul
        var uri = new Uri(address);
        var registration = new AgentServiceRegistration()
        {
            ID = "AuthService-1",
            Name = "AuthService",
            Address = uri.Host,
            Port = uri.Port,
            Tags = new[] { "Auth Service", "Auth" }
        };

        logger.LogInformation("Registering service with Consul..");
        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        consulClient.Agent.ServiceRegister(registration).Wait();

        lifetime.ApplicationStopping.Register(() =>
        {
            logger.LogInformation("Deregistering service from Consul..");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });

        return app;
    }
}