namespace CompilerTest.Configuration;

internal interface IConfigurationManager
{
    CPUConfiguration Configuration { get; }

    void Load(string path);
}