using Domain.Carts;
using Domain.Categories;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICartQueries
{
    Task<IReadOnlyList<Cart>> GetAll(CancellationToken cancellationToken);
    Task<Option<Cart>> GetById(CartId id, CancellationToken cancellationToken);
}