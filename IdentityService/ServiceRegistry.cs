using IdentityService.Application.Helpers;
using IdentityService.Application.Repositories;
using IdentityService.Application.Services.Auth;
using IdentityService.Infrastructures.Services.Auth;
using IdentityService.Infrastructures.Services.Queue.Kafka;
using IdentityService.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService;

public static class ServiceRegistry
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? jwtSecretKey = configuration["JwtSettings:SecretKey"];
        string? kafkaUrl = configuration["ServiceSettings:ServiceDiscoveryAddress"];

        if (string.IsNullOrEmpty(jwtSecretKey))
        {
            throw new ArgumentNullException(nameof(jwtSecretKey), "JWT Secret Key configuration is missing.");
        }

        if (string.IsNullOrEmpty(kafkaUrl))
        {
            throw new ArgumentNullException(nameof(kafkaUrl), "Kafka URL configuration is missing.");
        }

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<IUsersClaimsRepository, UsersClaimsRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();

        // Kafka Producer
        services.AddSingleton(provider =>
        {
            return new KafkaProducer(kafkaUrl);
        });
        //helpers
        services.AddSingleton<JwtHelper>();
        // Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return services;
    }
}
