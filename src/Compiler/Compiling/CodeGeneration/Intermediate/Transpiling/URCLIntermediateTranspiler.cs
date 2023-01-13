using System.Diagnostics;
using System.IO;

namespace CompilerTest.Compiling.CodeGeneration.Intermediate.Transpiling;

internal class URCLIntermediateTranspiler : IIntermediateTranspiler
{
    public string[] Transpile(string[] input, bool keepCode)
    {
        File.WriteAllLines("tmp.urcl", input);

        var process = Process.Start("python", new string[] { @"C:\URCL\urcl.py", "MyISA_raw", "tmp.urcl", "isa_output.txt", "tmp_urcl.urcl" });
        process.WaitForExit();

        var result = File.ReadAllLines("isa_output.txt");

        if(!keepCode)
            File.Delete("tmp.urcl");

        File.Delete("tmp_urcl.urcl");
        File.Delete("isa_output.txt");

        return result;
    }
}
