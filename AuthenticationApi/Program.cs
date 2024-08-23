using AuthenticationApi.Services.Queue.Kafka;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(provider =>
{
    return new KafkaProducer(builder.Configuration["KafkaSettings:Url"]);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var client = new ConsulClient();
var registration = new AgentServiceRegistration()
{
    ID = "authservice-1",
    Name = "AuthService",
    Address = "localhost",
    Port = 5280,
    Check = new AgentServiceCheck
    {
        HTTP = "http://localhost:5280/health",
        Interval = TimeSpan.FromSeconds(10),
        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
    }
};

await client.Agent.ServiceRegister(registration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("gatewayPolicy", opt => opt.WithOrigins("http://localhost:5019").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("gatewayPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
