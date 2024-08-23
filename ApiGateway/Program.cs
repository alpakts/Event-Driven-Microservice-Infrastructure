using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader());
});
builder.Services.AddOcelot().AddConsul();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDiscovery(options => options.UseConsul());
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");

    });
}
app.MapControllers();
await app.UseOcelot();

app.Run();
