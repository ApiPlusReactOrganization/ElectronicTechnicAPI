using Domain.Manufacturers;

namespace Application.Common.Interfaces.Queries;

public interface IManufacturerQueries
{
    Task<IReadOnlyList<Manufacturer>> GetAll(CancellationToken cancellationToken);
}