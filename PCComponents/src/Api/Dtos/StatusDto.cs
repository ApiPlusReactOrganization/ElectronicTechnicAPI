using Domain.Orders;

namespace Api.Dtos;

public record StatusDto(string? Name)
{
    public static StatusDto FromDomainModel(Status status)
    => new(status.Name);
}