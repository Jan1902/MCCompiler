using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.CodeGeneration.Target.IntermediateParsing
{
    internal interface IIntermediateParser
    {
        List<IntermediateInstruction> Parse(string[] input);
    }
}