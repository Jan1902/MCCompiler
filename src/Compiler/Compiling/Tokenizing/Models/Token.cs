namespace CompilerTest.Compiling.Tokenizing.Models;

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
    Asterisc,
    Percent,
    Slash,
    Colon,
    NewLine,
    KeyWord,
    Smaller,
    Bigger,
    Comma
}
