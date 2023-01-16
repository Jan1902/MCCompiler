using CommandLine;
using MCCompiler.Compiler.Abstraction;
using MCCompiler.Compiler.Lexer.Implementations;
using MCCompiler.CLI;
using Microsoft.Extensions.DependencyInjection;

Parser.Default.ParseArguments<BuildOptions>(args).WithParsed(Build);

void Build(BuildOptions options)
{
    var services = new ServiceCollection()
    .AddLogging()
    .AddSingleton(options)
    .AddScoped<Build>()
    .AddScoped<ITokenizer, Tokenizer>();

    var provider = services.BuildServiceProvider();
    provider.GetRequiredService<Build>().Run();
}