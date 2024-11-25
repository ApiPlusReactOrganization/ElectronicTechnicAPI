﻿using Domain.Authentications.Roles;
using Domain.Carts;
using Domain.Orders;

namespace Domain.Authentications.Users;

public class User
{
    public UserId Id { get; }
    public string Email { get; private set; }
    public string? Name { get; private set; }
    public string PasswordHash { get; }
    public UserImage? UserImage { get; private set; }
    public Cart? Cart { get; private set; }
    public List<Role> Roles { get; private set; } = new();
    // public List<Order> Orders { get; set; } = new();

    private User(UserId id, string email, string? name, string passwordHash)
    {
        Id = id;
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
    }

    public static User New(UserId id, string email, string? name, string passwordHash)
        => new(id, email, name, passwordHash);

    public void UpdateUser(string email, string? name)
    {
        Email = email;
        Name = name;
    }

    public void UpdateUserImage(UserImage userImage)
        => UserImage = userImage;

    public void SetRoles(List<Role> roles)
        => Roles = roles;

    public void SetCart(Cart cart)
        => Cart = cart;
}