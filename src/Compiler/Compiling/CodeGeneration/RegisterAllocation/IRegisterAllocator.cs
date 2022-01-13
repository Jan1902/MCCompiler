using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.CodeGeneration.RegisterAllocation
{
    internal interface IRegisterAllocator
    {
        List<IntermediateInstruction> AllocateRegisters(List<IntermediateInstruction> instructions);
    }
}