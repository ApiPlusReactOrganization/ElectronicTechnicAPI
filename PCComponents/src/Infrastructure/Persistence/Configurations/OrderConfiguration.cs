using Domain.Orders;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasConversion(m => m.Value, x => new OrderId(x));
            
            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .HasConstraintName("fk_orders_users_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Cart)
                .WithMany()
                .UsingEntity(x => x.ToTable("order_cards"));

            builder.Property(m => m.TotalPrice)
                .HasPrecision(9, 2);
            
            builder.Property(m => m.Status)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(m => m.DeliveryAddress)
                .HasMaxLength(200)
                .IsRequired();
            
            builder.Property(x => x.CreatedAt)
                .HasConversion(new DateTimeUtcConverter())
                .HasDefaultValueSql("timezone('utc', now())");
        }
    }
}