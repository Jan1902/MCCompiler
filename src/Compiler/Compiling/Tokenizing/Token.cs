using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Tokenizing
{
    internal class Token
    {
        public TokenType Type { get; set; }
        public string Content { get; set; }
        public int Line { get; set; }

        public Token(TokenType type, string content, int line)
        {
            Type = type;
            Content = content;
            Line = line;
        }

        public Token(TokenType type, char content, int line)
        {
            Type = type;
            Content = content.ToString();
            Line = line;
        }
    }

    internal enum TokenType
    {
        LeftBracket,
        RightBracket,
        LeftCurlyBracket,
        RightCurlyBracket,
        Number,
        Identifier,
        Equals,
        Plus,
        Minus,
        Colon,
        NewLine,
        KeyWord,
        Smaller,
        Bigger,
        Comma
    }
}
