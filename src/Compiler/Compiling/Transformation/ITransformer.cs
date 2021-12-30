using CompilerTest.Compiling.Parsing;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Transformation
{
    internal interface ITransformer
    {
        List<RawInstruction> Transform(Node node);
    }
}