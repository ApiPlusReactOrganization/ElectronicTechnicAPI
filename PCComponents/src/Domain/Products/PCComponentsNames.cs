namespace Domain.Products;

public class PCComponentsNames
{
    public const string CPU = "Processor";
    public const string Case = "Computer case";
    public const string GPU = "Graphics Card";
    public const string Motherboard = "Motherboard";
    public const string Psu = "Psu";
    public const string Ram = "Ram";
    public const string Cooler = "Cooler";
    public const string Hdd = "Hdd";
    public const string Sdd = "Sdd";
    
    public static readonly List<string> ListOfComponents = new()
    {
        CPU,
        Case,
        GPU,
        Motherboard,
        Psu,
        Ram,
        Cooler,
        Hdd,
        Sdd,
    };
}