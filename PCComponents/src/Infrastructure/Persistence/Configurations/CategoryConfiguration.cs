using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products.PCComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasConversion(m => m.Value, x => new CategoryId(x));
            builder.Property(m => m.Name).HasConversion(x => x.ToString(), x => SelectionCategoryType.From(x))
                .IsRequired().HasMaxLength(100);

            builder.HasMany(x => x.Manufacturers)
                .WithMany(x => x.Categories)
                .UsingEntity(x => x.ToTable("categories_manufacturers"));
        }
    }
}