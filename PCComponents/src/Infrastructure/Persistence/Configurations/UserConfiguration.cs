using Domain.Authentications.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(p => p.Value, x => new UserId(x));

        builder.Property(p => p.Name).HasMaxLength(25);

        builder.Property(p => p.Email).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity(x => x.ToTable("user_roles"));
            
        builder.HasMany(x => x.FavoriteProducts)
            .WithMany()
            .UsingEntity(x => x.ToTable("user_favorite_products"));
    }
}