using Domain.FormFactors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class FormFactorConfiguration : IEntityTypeConfiguration<FormFactor>
    {
        public void Configure(EntityTypeBuilder<FormFactor> builder)
        {
            builder.HasKey(ff => ff.Id);
            builder.Property(ff => ff.Id).HasConversion(ff => ff.Value, x => new FormFactorId(x));
            builder.Property(ff => ff.Name).IsRequired().HasMaxLength(100);
        }
    }
}