using CompilerTest.Compiling.Tokenizing;

namespace CompilerTest.Compiling.Parsing
{
    internal interface IParser
    {
        Node Parse(Token[] tokens);
    }
}