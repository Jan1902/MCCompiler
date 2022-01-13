using CompilerTest.Compiling.Analyzing;
using CompilerTest.Compiling.CodeGeneration.Intermediate;
using CompilerTest.Compiling.CodeGeneration.Intermediate.Transpiling;
using CompilerTest.Compiling.CodeGeneration.RegisterAllocation;
using CompilerTest.Compiling.CodeGeneration.Target;
using CompilerTest.Compiling.CodeGeneration.Target.IntermediateParsing;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Transformation;
using CompilerTest.Components;

namespace CompilerTest.Compiling
{
    internal class Compiler : ICompiler
    {
        private readonly IComponentProvider _componentProvider;

        public Compiler(IComponentProvider componentProvider)
        {
            _componentProvider = componentProvider;
        }

        public string[] Compile(string code)
        {
            ITokenizer tokenizer = _componentProvider.Tokenizer;
            var tokens = tokenizer.Tokenize(code);

            IParser parser = _componentProvider.Parser;
            var ast = parser.Parse(tokens);

            IAnalyzer analyzer = _componentProvider.Analyzer;
            ast = analyzer.AnalyzeAndCleanUp(ast);

            ITransformer transformer = _componentProvider.Transformer;
            var instructions = transformer.Transform(ast);

            IIntermediateTranslator intermediateTranslator = _componentProvider.IntermediateTranslator;
            var output = intermediateTranslator.Translate(instructions);

            IIntermediateTranspiler intermediateTranspiler = _componentProvider.IntermediateTranspiler;
            output = intermediateTranspiler.Transpile(output);

            IIntermediateParser intermediateParser = _componentProvider.IntermediateParser;
            instructions = intermediateParser.Parse(output);

            IRegisterAllocator registerAllocator = _componentProvider.RegisterAllocator;
            instructions = registerAllocator.AllocateRegisters(instructions);

            ITargetTranslator targetTranslator = _componentProvider.TargetTranslator;
            output = targetTranslator.Translate(instructions);

            return output;
        }
    }
}
