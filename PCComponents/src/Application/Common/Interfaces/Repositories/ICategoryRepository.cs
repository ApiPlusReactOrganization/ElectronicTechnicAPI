using Domain.Categories;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Option<Category>> GetById(CategoryId id, CancellationToken cancellationToken, bool includes = true);
    Task<Option<Category>> SearchByName(string name, CancellationToken cancellationToken);
    Task<Category> Add(Category сategory, CancellationToken cancellationToken);
    Task<Category> Update(Category сategory, CancellationToken cancellationToken);
    Task<Category> Delete(Category сategory, CancellationToken cancellationToken);
}