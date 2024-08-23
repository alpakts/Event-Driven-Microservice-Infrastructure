using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader());
});
// Ocelot'u servis olarak ekleyin
builder.Services.AddOcelot();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");

        c.SwaggerEndpoint("https://localhost:44361/swagger/v1/swagger.json", "Authentication API v1");
    });
}
app.MapControllers();
await app.UseOcelot();

app.Run();
