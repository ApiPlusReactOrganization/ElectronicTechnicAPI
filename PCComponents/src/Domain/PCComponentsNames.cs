namespace Domain;

public class PCComponentsNames
{
    public const string CPU = "Processor";
    public const string Case = "Computer case";
    public const string GPU = "Graphics Card";
    
    public static readonly List<string> ListOfComponents = new List<string>
    {
        CPU,
        Case,
        GPU
    };
}