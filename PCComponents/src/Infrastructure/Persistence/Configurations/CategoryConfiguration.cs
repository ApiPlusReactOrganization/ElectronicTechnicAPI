using Domain.Categories;
using Domain.Manufacturers;
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
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        }
    }
}