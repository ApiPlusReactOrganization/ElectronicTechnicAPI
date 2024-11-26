using Api.Dtos;
using Api.Modules.Errors;
using Application.CartItems.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Authentications.Users;
using Domain.CartItems;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("cart-items")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Roles = AuthSettings.AdminRole)]
[ApiController]
public class CartItemsController(ISender sender, ICartItemQueries cartItemQueries) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-all")]
    public async Task<ActionResult<IReadOnlyList<CartItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await cartItemQueries.GetAll(cancellationToken);

        return entities.Select(CartItemDto.FromDomainModel).ToList();
    }

    [HttpGet("get-by-id/{cartItemId:guid}")]
    public async Task<ActionResult<CartItemDto>> Get([FromRoute] Guid cartItemId, CancellationToken cancellationToken)
    {
        var entity = await cartItemQueries.GetById(
            new CartItemId(cartItemId), cancellationToken);

        return entity.Match<ActionResult<CartItemDto>>(
            ci => CartItemDto.FromDomainModel(ci),
            () => NotFound());
    }
    
    [HttpGet("get-by-user-id/{userId:guid}")]
    public async Task<ActionResult<IReadOnlyList<CartItemDto>>> GetByUser([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var entities = await cartItemQueries.GetByUserId(
            new UserId(userId), cancellationToken);

        return entities.Select(CartItemDto.FromDomainModel).ToList();
    }

    [HttpPost("create")]
    public async Task<ActionResult<CartItemDto>> Create(
        [FromBody] CartItemDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCartItemCommand
        {
            UserId = request.UserId!.Value,
            ProductId = request.ProductId!.Value,
            Quantity = request.Quantity,
        };
    
        var result = await sender.Send(input, cancellationToken);
    
        return result.Match<ActionResult<CartItemDto>>(
            ci => CartItemDto.FromDomainModel(ci),
            e => e.ToObjectResult());
    }

    [HttpPut("update")]
    public async Task<ActionResult<CartItemDto>> Update(
        [FromBody] CartItemDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCartItemCommand()
        {
            CartItemId = request.Id!.Value,
            Quantity = request.Quantity
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CartItemDto>>(
            ci => CartItemDto.FromDomainModel(ci),
            e => e.ToObjectResult());
    }

    [HttpDelete("delete/{cartItemId:guid}")]
    public async Task<ActionResult<CartItemDto>> 
        Delete([FromRoute] Guid cartItemId, CancellationToken cancellationToken)
    {
        var input = new DeleteCartItemCommand()
        {
            CartItemId = cartItemId
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CartItemDto>>(
            ci => CartItemDto.FromDomainModel(ci),
            e => e.ToObjectResult());
    }
}
