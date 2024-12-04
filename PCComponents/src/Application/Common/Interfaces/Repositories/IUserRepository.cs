using System.Collections.ObjectModel;
using Domain.Authentications.Users;
using Domain.Products;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> Create(User user, CancellationToken cancellationToken);
    Task<User> Update(User user, CancellationToken cancellationToken);
    Task<User> Delete(User user, CancellationToken cancellationToken);
    Task<User> AddRole(UserId userId, string idRole, CancellationToken cancellationToken);
    Task<Option<User>> GetById(UserId id, CancellationToken cancellationToken);
    Task<Option<User>> SearchByEmail(string email, CancellationToken cancellationToken);
    Task<Option<User>> SearchByEmailForUpdate(UserId userId, string email, CancellationToken cancellationToken);
    Task<User> AddFavoriteProduct(UserId userId, Product product, CancellationToken cancellationToken);
    Task<User> RemoveFavoriteProduct(UserId userId, Product product, CancellationToken cancellationToken);
}