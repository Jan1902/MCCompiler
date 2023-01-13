namespace MCCompiler.Compiler.Shared;

public class Token
{
    public TokenType Type { get; init; }
    public string Content { get; init; }
    public int Line { get; init; }

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

public enum TokenType
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
