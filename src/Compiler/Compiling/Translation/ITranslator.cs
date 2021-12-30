using CompilerTest.Compiling.Transformation;

namespace CompilerTest.Compiling.Translation
{
    internal interface ITranslator
    {
        string[] Translate(RawInstruction[] instructions);
    }
}