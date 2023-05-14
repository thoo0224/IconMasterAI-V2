using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Persistence.Entities;
using IconMasterAI.Persistence.Repositories;
using IconMasterAI.Persistence.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IconMasterAI.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
#if DEBUG
            options.UseSqlServer(configuration.GetConnectionString("Sql"));
#else
#endif
        });

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IMappingService, MappingService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(config =>
        {
            config.AddMaps(AssemblyReference.Assembly);
        });
        
        return services;
    }
}
