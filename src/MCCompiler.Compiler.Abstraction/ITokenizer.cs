using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Abstraction;

public interface ITokenizer
{
    Token[] Tokenize(string code);
}