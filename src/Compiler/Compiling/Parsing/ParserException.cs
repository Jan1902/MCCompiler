using CompilerTest.Compiling.Tokenizing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
