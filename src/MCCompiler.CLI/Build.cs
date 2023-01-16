using System.Runtime.CompilerServices;
using MCCompiler.Compiler.Abstraction;
using System;

namespace MCCompiler.CLI;

internal class Build
{
    private readonly BuildOptions _options;
    private readonly ITokenizer _tokenizer;

    public Build(BuildOptions options, ITokenizer tokenizer)
    {
        _options = options;
        _tokenizer = tokenizer;
    }

    public void Run()
    {
        var sourceCode = File.ReadAllText(_options.CodeFile);
        var tokens = _tokenizer.Tokenize(sourceCode);

        Console.WriteLine($"Parsed {tokens.Count()} tokens");
    }
}