using Domain.CartItems;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasConversion(m => m.Value, x => new CartItemId(x));
            
            builder.HasOne(m => m.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(m => m.CartId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(m => m.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  
            
            builder.Property(x => x.Quantity).IsRequired();
        }
    }
}