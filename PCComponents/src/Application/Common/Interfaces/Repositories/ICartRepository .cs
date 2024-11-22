using Domain.Carts;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Option<Cart>> GetById(CartId id, CancellationToken cancellationToken);
    /*
    Task<Option<Cart>> SearchByName(string name, CancellationToken cancellationToken);
    */
    Task<Cart> Add(Cart сategory, CancellationToken cancellationToken);
    Task<Cart> Update(Cart сategory, CancellationToken cancellationToken);
    Task<Cart> Delete(Cart сategory, CancellationToken cancellationToken);
}