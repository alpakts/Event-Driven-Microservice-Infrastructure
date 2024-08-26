using Consul;
using Ocelot.Configuration;
using Ocelot.Infrastructure.Extensions;
using Ocelot.Logging;
using Ocelot.Provider.Consul.Interfaces;
using Ocelot.Provider.Consul;
using Ocelot.Responses;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.ServiceDiscovery;
using Ocelot.Values;

namespace ApiGateway.ServiceProvider;

public class CustomConsulProviderForResolvingHost : IServiceDiscoveryProvider
{
    private const string VersionPrefix = "version-";
    private readonly ConsulRegistryConfiguration _config;
    private readonly IConsulClient _consul;
    private readonly IOcelotLogger _logger;

    public CustomConsulProviderForResolvingHost(ConsulRegistryConfiguration config, IOcelotLoggerFactory factory, IConsulClientFactory clientFactory)
    {
        _config = config;
        _consul = clientFactory.Get(_config);
        _logger = factory.CreateLogger<CustomConsulProviderForResolvingHost>();
    }

    public async Task<List<Service>> GetAsync()
    {
        var queryResult = await _consul.Health.Service(_config.KeyOfServiceInConsul, string.Empty, false);

        var services = new List<Service>();

        foreach (var serviceEntry in queryResult.Response)
        {
            if (IsValid(serviceEntry))
            {
                services.Add(BuildService(serviceEntry));
            }
            else
            {
                _logger.LogWarning($"Unable to use service Address: {serviceEntry.Service.Address} and Port: {serviceEntry.Service.Port} as it is invalid. Address must contain host only e.g. localhost and port must be greater than 0");
            }
        }

        return services.ToList();
    }

    private Service BuildService(ServiceEntry serviceEntry)
    {
        return new Service(
            serviceEntry.Service.Service,
            new ServiceHostAndPort(serviceEntry.Service.Address, serviceEntry.Service.Port),
            serviceEntry.Service.ID,
            GetVersionFromStrings(serviceEntry.Service.Tags),
            serviceEntry.Service.Tags ?? Enumerable.Empty<string>());
    }

    private bool IsValid(ServiceEntry serviceEntry)
    {
        if (string.IsNullOrEmpty(serviceEntry.Service.Address) || serviceEntry.Service.Address.Contains("http://") || serviceEntry.Service.Address.Contains("https://") || serviceEntry.Service.Port <= 0)
        {
            return false;
        }

        return true;
    }

    private string GetVersionFromStrings(IEnumerable<string> strings)
    {
        return strings
            ?.FirstOrDefault(x => x.StartsWith(VersionPrefix, StringComparison.Ordinal))
            .TrimStart(VersionPrefix);
    }
}

public class CustomConsulProviderFactory : IServiceDiscoveryProviderFactory
{
    /// <summary>
    /// String constant used for provider type definition.
    /// </summary>
    public const string PollConsul = nameof(CustomConsulProviderForResolvingHost);

    private static readonly List<PollConsul> ServiceDiscoveryProviders = new();
    private static readonly object LockObject = new();
    private IOcelotLoggerFactory _factory;
    private IServiceProvider _provider;

    public CustomConsulProviderFactory(IOcelotLoggerFactory factory, IServiceProvider provider)
    {
        _factory = factory;
        _provider = provider;
    }

    private IServiceDiscoveryProvider CreateProvider(
        ServiceProviderConfiguration config, DownstreamRoute route)
    {
        var factory = _provider.GetService<IOcelotLoggerFactory>();
        var consulFactory = _provider.GetService<IConsulClientFactory>();

        var consulRegistryConfiguration = new ConsulRegistryConfiguration(
            config.Scheme, config.Host, config.Port, route.ServiceName, config.Token);

        var consulProvider = new CustomConsulProviderForResolvingHost(consulRegistryConfiguration, factory, consulFactory);

        if (PollConsul.Equals(config.Type, StringComparison.OrdinalIgnoreCase))
        {
            lock (LockObject)
            {
                var discoveryProvider = ServiceDiscoveryProviders.FirstOrDefault(x => x.ServiceName == route.ServiceName);
                if (discoveryProvider != null)
                {
                    return discoveryProvider;
                }

                discoveryProvider = new PollConsul(config.PollingInterval, route.ServiceName, factory, consulProvider);
                ServiceDiscoveryProviders.Add(discoveryProvider);
                return discoveryProvider;
            }
        }

        return consulProvider;
    }

    Response<IServiceDiscoveryProvider> IServiceDiscoveryProviderFactory.Get(ServiceProviderConfiguration serviceConfig, DownstreamRoute route)
    {
        return new OkResponse<IServiceDiscoveryProvider>(CreateProvider(serviceConfig, route));
    }
}
