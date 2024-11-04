namespace Domain.Products.PCComponents;

public class PCComponentsManufactures
{
    public const string Intel = "Intel Corporation"; 
    public const string Nvidia = "NVIDIA Corporation"; 
    public const string Corsair = "Corsair Gaming, Inc."; 
    public const string Seagate = "Seagate Technology"; 
    public const string CoolerMaster = "Cooler Master Technology Inc."; 

    public static readonly List<string> ListOfManufacturers = new()
    {
        Intel,
        Nvidia,
        Corsair,
        Seagate,
        CoolerMaster
    };
}