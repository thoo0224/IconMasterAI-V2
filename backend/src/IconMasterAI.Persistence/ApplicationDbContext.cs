using IconMasterAI.Core.Entities;
using IconMasterAI.Persistence.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IconMasterAI.Persistence;

internal sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Icon>? Icons { get; init; }

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}