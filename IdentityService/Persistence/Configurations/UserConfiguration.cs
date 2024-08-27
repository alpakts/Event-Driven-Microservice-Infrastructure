using IdentityService.Application.Helpers;
using IdentityService.Persistence.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Persistence.Configurations;
public class UserConfiguration : BaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        base.Configure(builder);
        builder.Property(x => x.FullName).HasColumnName("FirstName").IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.Email).HasColumnName("Email").IsRequired(true).HasMaxLength(100);
        builder.HasIndex(x => x.Email, "UK_Users_Email").IsUnique();
        builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").IsRequired(true).HasMaxLength(500);
        builder.HasMany(u => u.UserClaims);
        builder.ToTable("Users");
        builder.HasData(getSeeds());

    }
    private IEnumerable<User> getSeeds()
    {
        List<User> users = new();

        HashingHelper.CreatePasswordHash(
            password: "password",
            passwordHash: out byte[] passwordHash,
            passwordSalt: out byte[] passwordSalt
        );
        User adminUser =
            new User()
            {
                Id = 1,
                FullName = "Super Admin",
                Email = "admin@admin.com",
                PasswordHash = passwordHash.ToString(),
                PasswordSalt = passwordSalt.ToString(),
                PhoneCountryCode = "90",
                PhoneNumber = "5555555555"
            };
        users.Add(adminUser);

        return users.ToArray();
    }
}