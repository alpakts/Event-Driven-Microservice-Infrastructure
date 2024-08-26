using ApiGateway.ServiceProvider;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Consul.Interfaces;
using Ocelot.ServiceDiscovery;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("ocelot.json")
                            .AddEnvironmentVariables()
                            .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RemoveAll<IServiceDiscoveryProviderFactory>();
builder.Services.AddSingleton<IServiceDiscoveryProviderFactory, CustomConsulProviderFactory>();
builder.Services.AddSingleton<ServiceDiscoveryFinderDelegate>((serviceProvider, config, downstreamRoute) => null);

builder.Services.AddSingleton<IConsulClientFactory>(new ConsulClientFactory());

builder.Services.AddOcelot(builder.Configuration).AddConfigStoredInConsul(); //Note: no .AddConsul() call!
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot();

app.Run();