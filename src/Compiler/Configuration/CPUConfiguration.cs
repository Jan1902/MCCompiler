using CompilerTest.Configuration.InstructionSet;
using System.Text.Json.Serialization;

namespace CompilerTest.Configuration;

internal class CPUConfiguration
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CPUArchitectureType Architecture { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BranchingType Branching { get; set; }

    public BasicInstructionSet InstructionSet { get; set; } = new BasicInstructionSet();

    public int RegisterCount { get; set; }
    public int MemorySize { get; set; }
    public int WordSize { get; set; }
}

public enum CPUArchitectureType
{
    ThreeOP,
    Accumulator,
    SUBLEQ
}

public enum BranchingType
{
    Default,
    Predicate
}
