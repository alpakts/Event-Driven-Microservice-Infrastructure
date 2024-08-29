using IdentityService.Domain.Entities;
using IdentityService.Domain.Entities.@base;
using IdentityService.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace IdentityService.Persistence.Context;
public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<User> Users { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.RegisterAllEntities<Entity>();
        modelBuilder.RegisterAllConfigurations(Assembly.GetExecutingAssembly());
        //todo savechanges method override
    }
    public override int SaveChanges()
    {
        var entities = ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;
            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

            if (entity.State == EntityState.Added)
            {
                ((Entity)entity.Entity).CreatedBy = user;
                ((Entity)entity.Entity).CreatedDate = now;
            }

            ((Entity)entity.Entity).ModifiedBy = user;
            ((Entity)entity.Entity).ModifiedDate = now;
        }
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var entities = ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow;
            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "anonymous";

            if (entity.State == EntityState.Added)
            {
                ((Entity)entity.Entity).CreatedBy = user;
                ((Entity)entity.Entity).CreatedDate = now;
            }

            ((Entity)entity.Entity).ModifiedBy = user;
            ((Entity)entity.Entity).ModifiedDate = now;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
