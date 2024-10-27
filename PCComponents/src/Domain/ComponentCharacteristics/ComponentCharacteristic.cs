namespace Domain.ComponentCharacteristics
{
    public class ComponentCharacteristic
    { 
        public string Type { get; }

        public Case? Case { get; private init; }
        public CPU? CPU { get; private init; }
        public Motherboard? Motherboard { get; private init; }
        public PSU? PSU { get; private init; }
        
        private ComponentCharacteristic(string type)
        {
            Type = type;
        }

        public static ComponentCharacteristic NewCase(Case someCase)
            => new("Computer case") { Case = someCase };
        public static ComponentCharacteristic NewCPU(CPU someCPU)
            => new("CPU") { CPU = someCPU };
        public static ComponentCharacteristic NewMotherboard(Motherboard someMotherboard)
            => new("Motherboard") { Motherboard = someMotherboard };
        public static ComponentCharacteristic NewPSU(PSU somePSU)
            => new("PSU") { PSU = somePSU };
    }
    
    public record Case
    {
        public required string FormFactors { get; init; }
        public required int NumberOfFans { get; init; } 
        public required string CoolingSystem { get; init; }
    }
    
    public record CPU
    {
        //make Socket and FormFactor models 
        public required string Socket { get; init; }
        public required int NumberOfCores { get; init; } 
        public required int NumberOfThreads { get; init; }
        public required string BaseClock { get; init; }
    }
    
    public record Motherboard
    {
        public required string Socket { get; init; }
        public required string FormFactors { get; init; }
        public required string RAMDescription { get; init; }
        public required string NetworkDescription { get; init; }
        public required string PowerDescription { get; init; }
        public required string AudioDescription { get; init; }
        public required string ExternalConnectorsDescription { get; init; } 
    }
    
    public record PSU
    {
        //maybe make field Protections like model
        public required string PowerCapacity { get; init; }
        public required string InputVoltageRange { get; init; }
        public required string FanTypeAndSize { get; init; }
        public required string Protections  { get; init; }
        public required string Connectors { get; init; }
    }
}
