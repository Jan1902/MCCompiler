using System.Collections.Generic;

namespace CompilerTest.Compiling.Transformation.Models;

internal class Block
{
    public List<IntermediateInstruction> Instructions { get; set; } = new List<IntermediateInstruction> { };
}
