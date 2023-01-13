using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Abstraction;

public interface IAnalyzer
{
    ASTNode AnalyzeAndCleanUp(ASTNode ast);
}