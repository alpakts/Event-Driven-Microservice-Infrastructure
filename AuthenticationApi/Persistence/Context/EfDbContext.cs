using IdentityService.Domain.Entities;
using IdentityService.Domain.Entities.@base;
using IdentityService.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
namespace IdentityService.Persistence.Context;
public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.RegisterAllEntities<Entity>();
        //todo savechanges method override
    }
}
