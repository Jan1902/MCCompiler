using CompilerTest.Compiling;
using CompilerTest.Compiling.Analyzing;
using CompilerTest.Compiling.CodeGeneration.Intermediate;
using CompilerTest.Compiling.CodeGeneration.Intermediate.Transpiling;
using CompilerTest.Compiling.CodeGeneration.RegisterAllocation;
using CompilerTest.Compiling.CodeGeneration.RegisterAllocation.Implementations;
using CompilerTest.Compiling.CodeGeneration.Target;
using CompilerTest.Compiling.CodeGeneration.Target.IntermediateParsing;
using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Parsing.Implementations;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Tokenizing.Implementations;
using CompilerTest.Compiling.Transformation;
using CompilerTest.Compiling.Transformation.Implementations;
using CompilerTest.Configuration;

namespace CompilerTest.Components.Implementations;

internal class ComponentProvider : IComponentProvider
{
    private readonly IConfigurationManager _configurationManager;
    private readonly CompilationEnvironment _compilationEnvironment;

    public ICompiler Compiler => ChooseCompiler();
    public ITokenizer Tokenizer => ChooseTokenizer();
    public IParser Parser => ChooseParser();
    public IAnalyzer Analyzer => ChooseAnalyzer();
    public ITransformer Transformer => ChooseTransformer();
    public IIntermediateTranslator IntermediateTranslator => ChooseIntermediateTranslator();
    public ITargetTranslator TargetTranslator => ChooseTargetTranslator();
    public IIntermediateParser IntermediateParser => ChooseIntermediateParser();
    public IIntermediateTranspiler IntermediateTranspiler => ChooseIntermediateTranspiler();
    public IRegisterAllocator RegisterAllocator => ChooseRegisterAllocator();

    public ComponentProvider(IConfigurationManager configurationManager)
    {
        _compilationEnvironment = new CompilationEnvironment();
        _configurationManager = configurationManager;
    }

    private ICompiler ChooseCompiler()
    {
        return new Compiler(this);
    }

    private ITokenizer ChooseTokenizer()
    {
        return new Tokenizer();
    }

    private IParser ChooseParser()
    {
        return new NewParser();
    }

    private IAnalyzer ChooseAnalyzer()
    {
        return new Analyzer();
    }

    private ITransformer ChooseTransformer()
    {
        return new Transformer(_compilationEnvironment);
    }

    private IIntermediateTranslator ChooseIntermediateTranslator()
    {
        return new URCLIntermediateTranslator(_compilationEnvironment);
    }

    private ITargetTranslator ChooseTargetTranslator()
    {
        return new TargetTranslator(_configurationManager.Configuration.InstructionSet);
    }

    private IIntermediateParser ChooseIntermediateParser()
    {
        return new URCLIntermediateParser(_compilationEnvironment);
    }

    private IIntermediateTranspiler ChooseIntermediateTranspiler()
    {
        return new URCLIntermediateTranspiler();
    }

    private IRegisterAllocator ChooseRegisterAllocator()
    {
        return new BasicRegisterAllocator();
    }
}
