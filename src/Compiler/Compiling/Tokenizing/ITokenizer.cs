using CompilerTest.Compiling.Tokenizing.Models;

namespace CompilerTest.Compiling.Tokenizing;

internal interface ITokenizer
{
    Token[] Tokenize(string code);
}