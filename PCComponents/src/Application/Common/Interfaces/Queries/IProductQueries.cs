using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IProductQueries
{
    Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken);

    Task<Option<Product>> GetById(ProductId id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Product>> GetProductsByCategoryAndManufacturer(
        CategoryId categoryId,
        ManufacturerId manufacturerId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Product>> GetProductsByManufacturer(
        ManufacturerId manufacturerId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Product>> GetProductsByCategory(
        CategoryId category,
        CancellationToken cancellationToken);
}