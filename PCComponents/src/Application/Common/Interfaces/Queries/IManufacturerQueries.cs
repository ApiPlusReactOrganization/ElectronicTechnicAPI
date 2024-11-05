using Domain.Authentications.Users;
using Domain.Manufacturers;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IManufacturerQueries
{
    Task<IReadOnlyList<Manufacturer>> GetAll(CancellationToken cancellationToken);
    Task<Option<Manufacturer>> GetById(ManufacturerId id, CancellationToken cancellationToken);
}