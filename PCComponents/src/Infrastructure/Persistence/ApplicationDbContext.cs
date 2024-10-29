using System.Reflection;
using Domain.Categories;
using Domain.FormFactors;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Sockets;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<FormFactor> FormFactors { get; set; }
    public DbSet<Socket> Sockets { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        
        builder.Seed();
    }
}