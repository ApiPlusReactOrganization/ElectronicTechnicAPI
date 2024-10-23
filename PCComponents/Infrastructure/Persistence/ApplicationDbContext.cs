using System.Reflection;
using Domain.Cases;
using Domain.FormFactors;
using Domain.Manufacturers;
using Domain.ProductMaterials;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Case> Cases { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<FormFactor> FormFactors { get; set; }
    public DbSet<ProductMaterial> ProductMaterials { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}