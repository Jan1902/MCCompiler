using CompilerTest.Compiling;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Transformation;
using CompilerTest.Compiling.Translation;

namespace CompilerTest.Components
{
    internal interface IComponentProvider
    {
        ICompiler Compiler { get; }
        IParser Parser { get; }
        ITokenizer Tokenizer { get; }
        ITransformer Transformer { get; }
        ITranslator Translator { get; }
    }
}