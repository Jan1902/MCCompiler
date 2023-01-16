using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Parser;

internal class ParserException : Exception
{
    public Token? Token { get; }

    public ParserException(Token? token, string message) : base(message)
        => Token = token;
}
