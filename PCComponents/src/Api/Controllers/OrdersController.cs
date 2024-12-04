using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Orders.Commands;
using Domain.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("orders")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Roles = AuthSettings.UserRole)]
[ApiController]
public class OrdersController(ISender sender, IOrderQueries orderQueries, IStatusQueries statusQueries) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await orderQueries.GetAll(cancellationToken);

        return entities.Select(OrderDto.FromDomainModel).ToList();
    }
    
    [HttpGet("get-all-status")]
    public async Task<ActionResult<IReadOnlyList<StatusDto>>> GetAllStatuses(CancellationToken cancellationToken)
    {
        var entities = await statusQueries.GetAllStatuses(cancellationToken);

        return entities.Select(StatusDto.FromDomainModel).ToList();
    }

    [HttpPost("create")]
    public async Task<ActionResult<OrderDto>> Create([FromBody] OrderDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateOrderCommand()
        {
            UserId = request.UserId!.Value,
            DeliveryAddress = request.DeliveryAddress,
        };
    
        var result = await sender.Send(input, cancellationToken);
    
        return result.Match<ActionResult<OrderDto>>(
            order => OrderDto.FromDomainModel(order),
            e => e.ToObjectResult());
    }
    
    [HttpPut("update-status/{orderId:guid}")]
    public async Task<ActionResult<OrderDto>> UpdateStatus(
        [FromRoute] Guid orderId,
        [FromBody] StatusDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateStatusForOrderCommand()
        {
            OrderId = orderId,
            StatusId = request.Name!,
        };
    
        var result = await sender.Send(input, cancellationToken);
    
        return result.Match<ActionResult<OrderDto>>(
            order => OrderDto.FromDomainModel(order),
            e => e.ToObjectResult());
    }
}