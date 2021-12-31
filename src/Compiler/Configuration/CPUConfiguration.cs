using CompilerTest.Compiling.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CompilerTest.Configuration
{
    internal class CPUConfiguration
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CPUArchitectureType Architecture { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BranchingType Branching { get; set; }

        public InstructionSet InstructionSet { get; set; } = new InstructionSet();
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
}
