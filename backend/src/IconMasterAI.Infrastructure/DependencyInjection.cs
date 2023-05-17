using System.Text;

using IconMasterAI.Core.Services;
using IconMasterAI.Core.Services.Icon;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Infrastructure.Services;
using IconMasterAI.Infrastructure.Services.Security;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IconMasterAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Google:ClientId"] ?? throw new Exception("No Google client id in configuration.");
                options.ClientSecret = configuration["Google:ClientSecret"] ?? throw new Exception("No google secret in configuration.");
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });

        services.AddHttpClient("openai", options =>
        {
            options.BaseAddress = new Uri("https://api.openai.com");
            options.DefaultRequestHeaders.Add(
                "Authorization", $"Bearer {configuration["OpenAi:ApiKey"]}");
        });

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserAccessorService, UserAccessorService>();

        services.AddScoped<IImageGenerationService, OpenAIImageGenerationService>();
        services.AddScoped<IIconGenerationService, IconGenerationService>();

        return services;
    }
}
