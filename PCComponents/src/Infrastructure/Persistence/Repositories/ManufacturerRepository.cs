using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Manufacturers;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ManufacturerRepository(ApplicationDbContext context) : IManufacturerRepository, IManufacturerQueries
{
    public async Task<Option<Manufacturer>> GetByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity == null ? Option.None<Manufacturer>() : Option.Some(entity);
    }

    public async Task<IReadOnlyList<Manufacturer>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Manufacturers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Manufacturer>> GetById(ManufacturerId id, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Manufacturer>() : Option.Some(entity);
    }

    public async Task<Manufacturer> Add(Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        await context.Manufacturers.AddAsync(manufacturer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return manufacturer;
    }

    public async Task<Manufacturer> Update(Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        context.Manufacturers.Update(manufacturer);

        await context.SaveChangesAsync(cancellationToken);

        return manufacturer;
    }

    public async Task<Manufacturer> Delete(Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        context.Manufacturers.Remove(manufacturer);

        await context.SaveChangesAsync(cancellationToken);

        return manufacturer;
    }
}