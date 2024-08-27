using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IdentityService.Domain.Entities.@base;

namespace IdentityService.Persistence.Configurations.Base;
public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired(true).ValueGeneratedOnAdd();
        builder.Property(x => x.ModifiedDate).HasColumnName("ModifiedDate").IsRequired(false);
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired(true).HasDefaultValue(false);
        builder.Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsRequired(false);
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}