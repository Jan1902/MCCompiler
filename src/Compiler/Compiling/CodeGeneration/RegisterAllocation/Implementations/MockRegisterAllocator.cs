using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.CodeGeneration.RegisterAllocation.Implementations;

internal class MockRegisterAllocator : IRegisterAllocator
{
    public List<IntermediateInstruction> AllocateRegisters(List<IntermediateInstruction> instructions)
    {
        return instructions;
    }
}
