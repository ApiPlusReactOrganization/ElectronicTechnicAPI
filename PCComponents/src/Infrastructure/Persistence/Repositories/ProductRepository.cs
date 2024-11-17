using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository, IProductQueries
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetProductsByCategoryAndManufacturer(
        CategoryId category,
        ManufacturerId manufacturerId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => p.CategoryId == category && p.ManufacturerId == manufacturerId)
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyList<Product>> GetProductsByManufacturer(
        ManufacturerId manufacturerId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => p.ManufacturerId == manufacturerId)
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetProductsByCategory(
        CategoryId category,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => p.CategoryId == category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Product>> GetById(ProductId id, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .Include(p => p.Manufacturer)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Product>() : Option.Some(entity);
    }

    public async Task<Option<Product>> SearchByName(string productName, CategoryId categoryId,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == productName && x.CategoryId == categoryId, cancellationToken);

        return entity == null ? Option.None<Product>() : Option.Some(entity);
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Product> Add(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product> Update(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product> Delete(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Option<Product>> SearchByNameAndDifferentFields(
        string name,
        decimal price,
        string description,
        int stockQuantity,
        CategoryId categoryId,
        ManufacturerId manufacturerId,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .Where(p => p.Name == name
                        && p.CategoryId == categoryId
                        && p.ManufacturerId == manufacturerId
                        && p.Price == price
                        && p.Description == description
                        && p.StockQuantity == stockQuantity)
            .FirstOrDefaultAsync(cancellationToken);

        return entity == null ? Option.None<Product>() : Option.Some(entity);
    }
}