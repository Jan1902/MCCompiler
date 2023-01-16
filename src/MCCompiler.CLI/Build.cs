using MCCompiler.Compiler.Abstraction;

namespace MCCompiler.CLI;

internal class Build
{
    private readonly BuildOptions _options;
    private readonly ITokenizer _tokenizer;
    private readonly IParser _parser;

    public Build(BuildOptions options, ITokenizer tokenizer, IParser parser)
    {
        _options = options;
        _tokenizer = tokenizer;
        _parser = parser;
    }

    public void Run()
    {
        var sourceCode = File.ReadAllText(_options.CodeFile);

        var tokens = _tokenizer.Tokenize(sourceCode);
        var ast = _parser.Parse(tokens);
    }
}