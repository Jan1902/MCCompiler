using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Abstraction;

public interface IParser
{
    ASTNode Parse(Token[] tokens);
}