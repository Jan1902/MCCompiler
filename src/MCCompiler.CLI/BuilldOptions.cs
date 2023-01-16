using CommandLine;

namespace MCCompiler.CLI;

[Verb("build", HelpText = "Compile the given source code")]
internal class BuildOptions
{
    [Option('c', "code-file", Required = true, HelpText = "The name of the code file to compile.")]
    public string CodeFile { get; }

    [Option('o', "output-file", Required = true, HelpText = "The name of the file to output to.")]
    public string OutputFile { get; }

    [Option('f', "config-file", Required = true, HelpText = "The name of the config file to use.")]
    public string ConfigFile { get; }

    public BuildOptions(string codeFile, string outputFile, string configFile)
    {
        CodeFile = codeFile;
        OutputFile = outputFile;
        ConfigFile = configFile;
    }
}
