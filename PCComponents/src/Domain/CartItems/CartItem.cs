using Domain.Authentications.Users;
using Domain.Orders;
using Domain.Products;

namespace Domain.CartItems;

public class CartItem
{
    public CartItemId Id { get; set; }

    public User User { get; }
    
    public UserId UserId { get; }

    public ProductId ProductId { get; private set; }
    
    public Product? Product { get; set; }

    public int Quantity { get; private set; }
    
    public bool IsFinished { get; private set; } = false;
    
    // public List<Order> Orders { get; set; } = new();

    private CartItem(CartItemId id, UserId userId, ProductId productId, int quantity)
    {
        Id = id;
        UserId = userId;
        ProductId = productId;
        Quantity = quantity;
    }

    public static CartItem New(CartItemId id, UserId userId, ProductId productId, int quantity)
        => new CartItem(id, userId, productId, quantity);
    
    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
    
    public void Finish()
     => IsFinished = true;
}