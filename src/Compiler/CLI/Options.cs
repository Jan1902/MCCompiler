using CommandLine;

namespace CompilerTest.CLI;

internal class Options
{
    [Option('c', "code-file", Required = true, HelpText = "The name of the code file to compile.")]
    public string CodeFile { get; set; }

    [Option('o', "output-file", Required = true, HelpText = "The name of the file to output to.")]
    public string OutputFile { get; set; }

    [Option('f', "config-file", Required = true, HelpText = "The name of the config file.")]
    public string ConfigFile { get; set; }
}
