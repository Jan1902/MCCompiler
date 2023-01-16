using CommandLine;
using MCCompiler.Compiler.Abstraction;
using MCCompiler.Compiler.Lexer.Implementations;
using MCCompiler.CLI;
using Microsoft.Extensions.DependencyInjection;

CommandLine.Parser.Default.ParseArguments<BuildOptions>(args).WithParsed(Build);

static void Build(BuildOptions options)
{
    var services = new ServiceCollection()
    .AddLogging()
    .AddSingleton(options)
    .AddScoped<Build>()
    .AddScoped<ITokenizer, Tokenizer>()
    .AddScoped<IParser, MCCompiler.Compiler.Parser.Implementations.Parser>();

    var provider = services.BuildServiceProvider();
    provider.GetRequiredService<Build>().Run();
}