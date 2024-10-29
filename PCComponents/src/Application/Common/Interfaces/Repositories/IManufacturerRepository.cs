using Domain.Manufacturers;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IManufacturerRepository
{
    Task<Option<Manufacturer>> GetById(ManufacturerId id, CancellationToken cancellationToken);
    Task<Option<Manufacturer>> SearchByName(string name, CancellationToken cancellationToken);
    Task<Manufacturer> Add(Manufacturer manufacturer, CancellationToken cancellationToken);
    Task<Manufacturer> Update(Manufacturer manufacturer, CancellationToken cancellationToken);
    Task<Manufacturer> Delete(Manufacturer manufacturer, CancellationToken cancellationToken);
}