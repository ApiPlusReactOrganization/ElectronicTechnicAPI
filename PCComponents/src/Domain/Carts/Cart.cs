using Domain.Authentications.Users;
using Domain.CartItems;

namespace Domain.Carts;

public class Cart
{
    public Guid Id { get; set; } 
    
    public UserId UserId { get; set; } 
    public User User { get; set; }
    
    public List<CartItem> Items { get; set; } = new(); 
}