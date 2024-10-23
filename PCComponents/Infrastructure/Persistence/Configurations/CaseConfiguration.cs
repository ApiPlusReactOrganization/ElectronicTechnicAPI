using Domain.Cases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CaseConfiguration : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            // Налаштування специфічних властивостей
            builder.Property(c => c.NumberOfFans).IsRequired();
            builder.Property(c => c.CoolingSystem).HasMaxLength(100);

            // Налаштування зв'язку з Manufacturer
            builder.HasOne(c => c.Manufacturer)
                .WithMany(m => m.Cases)
                .HasForeignKey(c => c.ManufacturerId);

            // Налаштування зв'язку з FormFactor
            builder.HasMany(c => c.FormFactors)
                .WithMany(f => f.Cases)
                .UsingEntity(j => j.ToTable("CaseFormFactors"));
        }
    }
}