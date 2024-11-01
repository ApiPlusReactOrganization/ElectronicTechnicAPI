using Domain.Products;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Option<Product>> GetById(ProductId id, CancellationToken cancellationToken);
    Task<Option<Product>> SearchByName(string name, CancellationToken cancellationToken);
    Task<Product> Add(Product product, CancellationToken cancellationToken);
    Task<Product> Update(Product product, CancellationToken cancellationToken);
    Task<Product> Delete(Product product, CancellationToken cancellationToken);
}