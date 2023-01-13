using CompilerTest.Compiling.Parsing.Models;

namespace CompilerTest.Compiling.Analyzing;

internal interface IAnalyzer
{
    ASTNode AnalyzeAndCleanUp(ASTNode ast);
}