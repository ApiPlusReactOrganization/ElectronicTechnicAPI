namespace Domain.Orders;

public class Status
{
    public string Id { get; set; }
    public string Name { get; set; }

    private Status(string name)
    {
        Id = name;
        Name = name;
    }

    public static Status New(string name)
        => new(name);
}