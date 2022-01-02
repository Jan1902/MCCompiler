using CompilerTest.Compiling.Transformation;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Translation
{
    internal interface ITranslator
    {
        string[] Translate(List<IntermediateInstruction> instructions);
    }
}