using Domain.Authentications.Users;
using Domain.Products;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IUserQueries
{
    Task<IReadOnlyList<User>> GetAll(CancellationToken cancellationToken);
    Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken);
    Task<Option<User>> SearchByEmail(string email, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetFavoriteProductsByUserId(UserId userId, CancellationToken cancellationToken);
}