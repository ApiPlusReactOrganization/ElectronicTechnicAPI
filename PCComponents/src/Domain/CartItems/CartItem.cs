using Domain.Carts;
using Domain.Products;

namespace Domain.CartItems;

public class CartItem
{
    public CartItemId Id { get; set; }

    public CartId CartId { get; private set; }
    
    public Cart? Cart { get; set; }

    public ProductId ProductId { get; private set; }
    
    public Product? Product { get; set; }

    public int Quantity { get; private set; }

    private CartItem(CartItemId id, CartId cartId, ProductId productId, int quantity)
    {
        Id = id;
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
    }

    public static CartItem New(CartItemId id, CartId cartId, ProductId productId, int quantity)
        => new CartItem(id, cartId, productId, quantity);
    
    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}