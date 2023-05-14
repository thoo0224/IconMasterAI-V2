using System.Text.Json.Serialization;

using IconMasterAI.Application;
using IconMasterAI.Infrastructure;
using IconMasterAI.Middlewares;
using IconMasterAI.Persistence;

using Microsoft.OpenApi.Models;

namespace IconMasterAI;

public sealed class Startup
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddPolicy("cors", corsBuilder =>
                {
                    var frontendUrl = _configuration["AppSettings:FrontendUrl"]
                                      ?? throw new Exception("Could not find frontend in app settings.");

                    corsBuilder.WithOrigins(frontendUrl)
                        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
        }

        services
            .AddApplication()
            .AddInfrastructure(_configuration)
            .AddPersistence(_configuration);

        // TODO: Secure
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
        });

        services.AddHttpClient();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

#if DEBUG
        app.UseHttpsRedirection();
#endif

        app.UseRouting();
        app.UseCors("cors");

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        // TODO: Secure
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
