namespace Domain.Orders;

public static class StatusesConstants
{
    public const string Processing = "Processing";
    public const string Delivered = "Delivered";
    
    public static readonly List<string> ListOfStatuses = new()
    {
        Processing,
        Delivered
    };
}