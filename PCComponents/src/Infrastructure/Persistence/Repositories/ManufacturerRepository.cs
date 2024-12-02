using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Manufacturers;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class ManufacturerRepository : IManufacturerRepository, IManufacturerQueries
{
    private readonly ApplicationDbContext _context;

    public ManufacturerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<Manufacturer>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Manufacturers
            .Include(x=>x.Categories)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Option<Manufacturer>> GetById(ManufacturerId id, CancellationToken cancellationToken)
    {
        var entity = await _context.Manufacturers
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Manufacturer>() : Option.Some(entity);
    }
    public async Task<Option<Manufacturer>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await _context.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity == null ? Option.None<Manufacturer>() : Option.Some(entity);
    }

    public async Task<Manufacturer> Add(Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        await _context.Manufacturers.AddAsync(manufacturer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return manufacturer;
    }

    public async Task<Manufacturer> Update(Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        _context.Manufacturers.Update(manufacturer);
        await _context.SaveChangesAsync(cancellationToken);
        return manufacturer;
    }

    public async Task<Manufacturer> Delete(Manufacturer manufacturer, CancellationToken cancellationToken)
    {
        _context.Manufacturers.Remove(manufacturer);
        await _context.SaveChangesAsync(cancellationToken);
        return manufacturer;
    }
}