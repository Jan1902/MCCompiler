using CompilerTest.Compiling.Tokenizing.Models;
using System;

namespace CompilerTest.Compiling.Parsing
{
    internal class ParserException : Exception
    {
        public Token Token { get; }

        public ParserException(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}
