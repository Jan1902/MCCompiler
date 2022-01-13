using CompilerTest.Compiling.Parsing.Models;
using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Transformation
{
    internal interface ITransformer
    {
        List<IntermediateInstruction> Transform(ASTNode node);
    }
}