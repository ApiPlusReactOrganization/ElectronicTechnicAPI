namespace Domain.Products;

public class PCComponentsNames
{
    public const string CPU = "Processor";
    public const string Case = "Computer case";
    public const string GPU = "Graphics Card";
    public const string RAM = "RAM";
    public const string PSU = "Power Supply Unit";
    public const string Motherboard = "Motherboard";
    
    public static readonly List<string> ListOfComponents = new()
    {
        CPU,
        Case,
        GPU,
        RAM,
        PSU,
        Motherboard
    };
}