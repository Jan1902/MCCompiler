using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.CodeGeneration.Intermediate;

internal interface IIntermediateTranslator
{
    string[] Translate(List<IntermediateInstruction> instructions);
}