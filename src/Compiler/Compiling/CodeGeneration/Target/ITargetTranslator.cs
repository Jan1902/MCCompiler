using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.CodeGeneration.Target
{
    internal interface ITargetTranslator
    {
        string[] Translate(List<IntermediateInstruction> instructions);
    }
}