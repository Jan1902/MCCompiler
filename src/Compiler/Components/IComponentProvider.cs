using CompilerTest.Compiling;
using CompilerTest.Compiling.Analyzing;
using CompilerTest.Compiling.CodeGeneration.Intermediate;
using CompilerTest.Compiling.CodeGeneration.Intermediate.Transpiling;
using CompilerTest.Compiling.CodeGeneration.RegisterAllocation;
using CompilerTest.Compiling.CodeGeneration.Target;
using CompilerTest.Compiling.CodeGeneration.Target.IntermediateParsing;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Transformation;

namespace CompilerTest.Components
{
    internal interface IComponentProvider
    {
        ICompiler Compiler { get; }
        ITokenizer Tokenizer { get; }
        IParser Parser { get; }
        IAnalyzer Analyzer { get; }
        ITransformer Transformer { get; }
        IIntermediateTranslator IntermediateTranslator { get; }
        IIntermediateTranspiler IntermediateTranspiler { get; }
        IIntermediateParser IntermediateParser { get; }
        ITargetTranslator TargetTranslator { get; }
        IRegisterAllocator RegisterAllocator { get; }
    }
}