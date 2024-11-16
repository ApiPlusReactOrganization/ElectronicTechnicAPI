using System.Reflection;
using Application.Services.HashPasswordService;
using Domain.Authentications.Roles;
using Domain.Authentications.Users;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHashPasswordService hashPasswordService) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        DataSeed.Seed(builder, hashPasswordService);
    }
}