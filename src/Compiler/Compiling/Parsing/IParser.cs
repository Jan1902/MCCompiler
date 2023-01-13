using CompilerTest.Compiling.Parsing.Models;
using CompilerTest.Compiling.Tokenizing.Models;

namespace CompilerTest.Compiling.Parsing;

internal interface IParser
{
    ASTNode Parse(Token[] tokens);
}