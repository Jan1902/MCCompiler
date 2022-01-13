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

namespace CompilerTest.Components.Implementations
{
    internal class TestComponentProvider : IComponentProvider
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly CompilationEnvironment _compilationEnvironment;

        public ICompiler Compiler => ChooseCompiler();
        public ITokenizer Tokenizer => ChooseTokenizer();
        public IParser Parser => ChooseParser();
        public IAnalyzer Analyzer => ChooseAnalyzer();
        public ITransformer Transformer => ChooseTransformer();
        public ITargetTranslator TargetTranslator => ChooseTargetTranslator();
        public IIntermediateTranslator IntermediateTranslator => ChooseIntermediateTranslator();
        public IIntermediateTranspiler IntermediateTranspiler => ChooseIntermediateTranspiler();
        public IIntermediateParser IntermediateParser => ChooseIntermediateParser();
        public IRegisterAllocator RegisterAllocator => ChooseRegisterAllocator();

        public TestComponentProvider(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _compilationEnvironment = new CompilationEnvironment();
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
            return new MockParser();
        }

        private IAnalyzer ChooseAnalyzer()
        {
            return new Analyzer();
        }

        private ITransformer ChooseTransformer()
        {
            return new Transformer(_compilationEnvironment);
        }

        private ITargetTranslator ChooseTargetTranslator()
        {
            return new TargetTranslator(_configurationManager.Configuration.InstructionSet);
        }

        private IIntermediateTranslator ChooseIntermediateTranslator()
        {
            return new URCLIntermediateTranslator(_compilationEnvironment);
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
}
