﻿using Domain.Categories;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICategoryQueries
{
    Task<IReadOnlyList<Category>> GetAll(CancellationToken cancellationToken);
    Task<Option<Category>> GetById(CategoryId id, CancellationToken cancellationToken, bool includes = true);
}