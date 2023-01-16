using CommandLine;

namespace MCCompiler.CLI;

[Verb("build", HelpText = "Compile the given source code")]
internal class BuildOptions
{
    [Option('s', "source-file", Required = true, HelpText = "The name of the source code file to compile.")]
    public string CodeFile { get; }

    [Option('o', "output-file", Required = true, HelpText = "The name of the file to output to.")]
    public string OutputFile { get; }

    [Option('c', "config-file", Required = true, HelpText = "The name of the config file to use.")]
    public string ConfigFile { get; }

    public BuildOptions(string codeFile, string outputFile, string configFile)
    {
        CodeFile = codeFile;
        OutputFile = outputFile;
        ConfigFile = configFile;
    }
}
