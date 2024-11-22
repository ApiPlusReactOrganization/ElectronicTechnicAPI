using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Domain.Products.PCComponents;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Option<Product>> GetById(ProductId id, CancellationToken cancellationToken);
    Task<Option<Product>> SearchByName(string productName, CategoryId categoryId, CancellationToken cancellationToken);
    Task<Product> Add(Product product, CancellationToken cancellationToken);
    Task<Product> Update(Product product, CancellationToken cancellationToken);
    Task<Product> Delete(Product product, CancellationToken cancellationToken);

    Task<Option<Product>> SearchByNameAndDifferentFields(
        string name,
        decimal price,
        string description,
        int stockQuantity,
        CategoryId categoryId,
        ManufacturerId manufacturerId,
        CancellationToken cancellationToken);

    Task<int> SaveChanges(CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken);
}