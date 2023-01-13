using CompilerTest.Compiling.Transformation.Enums;

namespace CompilerTest.Compiling.Transformation.Models;

internal class IntermediateInstruction
{
    public Operations Operation { get; set; }
    public object[] Parameters { get; set; }

    public IntermediateInstruction(Operations operation, params object[] parameters)
    {
        Operation = operation;
        Parameters = parameters;
    }

    public IntermediateInstruction()
    {

    }
}
