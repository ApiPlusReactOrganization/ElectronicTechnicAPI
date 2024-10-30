namespace Domain.Products
{
    public class ComponentCharacteristic
    { 
        
        public Case? Case { get; set; }
        public CPU? Cpu { get; set; }
        public GPU? Gpu { get; set; }
        

        public static ComponentCharacteristic NewCase(Case someCase)
            => new () { Case = someCase };
        
        public static ComponentCharacteristic NewCpu(CPU cpu)
            => new() { Cpu = cpu };

        public static ComponentCharacteristic NewGpu(GPU gpu)
            => new() { Gpu = gpu };
    }

    public record Case
    {
        public required string FormFactors { get; init; }
        public required int NumberOfFans { get; init; } 
        public required string CoolingSystem { get; init; }
    }

    public record CPU
    {
        public required string Model { get; init; }
        public required int Cores { get; init; }
        public required int Threads { get; init; }
        public required double BaseClock { get; init; } // GHz
        public required double BoostClock { get; init; } // GHz
    }

    public record GPU
    {
        public required string Model { get; init; }
        public required int MemorySize { get; init; } // GB
        public required string MemoryType { get; init; }
        public required int CoreClock { get; init; } // MHz
        public required int BoostClock { get; init; } // MHz
    }
}
