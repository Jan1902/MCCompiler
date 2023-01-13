namespace CompilerTest.Compiling.CodeGeneration.Intermediate.Transpiling;

internal interface IIntermediateTranspiler
{
    string[] Transpile(string[] input, bool keepCode);
}