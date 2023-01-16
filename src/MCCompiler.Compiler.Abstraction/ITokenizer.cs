using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Abstraction;

public interface ITokenizer
{
    IEnumerable<Token> Tokenize(string code);
}