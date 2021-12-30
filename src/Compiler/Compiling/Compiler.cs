using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Transformation;
using CompilerTest.Compiling.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling
{
    internal class Compiler : ICompiler
    {
        private readonly IInstructionSet _instructionSet;

        public Compiler(IInstructionSet instructionSet)
        {
            _instructionSet = instructionSet;
        }

        public string[] Compile(string code)
        {
            var environment = new CompilationEnvironment();

            ITokenizer tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(code);

            IParser parser = new Parser(tokens);
            var ast = parser.Parse();

            ITransformer transformer = new Transformer(environment);
            var instructions = transformer.Transform(ast).ToArray();

            ITranslator translator = new Translator(_instructionSet);
            var output = translator.Translate(instructions);

            return output.ToArray();
        }
    }
}
