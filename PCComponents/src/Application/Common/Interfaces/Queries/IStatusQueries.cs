using Domain.Orders;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IStatusQueries
{
    Task<IReadOnlyList<Status>> GetAllStatuses(CancellationToken cancellationToken);
    Task<Option<Status>> GetStatusByName(string statusName, CancellationToken cancellationToken);
}