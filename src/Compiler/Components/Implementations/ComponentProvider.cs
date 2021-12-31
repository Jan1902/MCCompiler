using CompilerTest.Compiling;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Parsing.Implementations;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Tokenizing.Implementations;
using CompilerTest.Compiling.Transformation;
using CompilerTest.Compiling.Transformation.Implementations;
using CompilerTest.Compiling.Translation;
using CompilerTest.Compiling.Translation.Implementations;
using CompilerTest.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest
{
    internal class ComponentProvider : IComponentProvider
    {
        private readonly IConfigurationManager _configurationManager;

        public ICompiler Compiler { get => ChooseCompiler(); }
        public ITokenizer Tokenizer { get => ChooseTokenizer(); }
        public IParser Parser { get => ChooseParser(); }
        public ITransformer Transformer { get => ChooseTransformer(); }
        public ITranslator Translator { get => ChooseTranslator(); }

        public ComponentProvider(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        private ICompiler ChooseCompiler()
        {
            return new Compiler(_configurationManager.Configuration, this);
        }

        private ITokenizer ChooseTokenizer()
        {
            return new Tokenizer();
        }

        private IParser ChooseParser()
        {
            return new Parser();
        }

        private ITransformer ChooseTransformer()
        {
            if (_configurationManager.Configuration.Architecture == CPUArchitectureType.ThreeOP)
                return new Transformer();

            throw new Exception("Couldn't find a Transformer implementation that is compatible with the given CPU Architecture");
        }

        private ITranslator ChooseTranslator()
        {
            return new Translator(_configurationManager.Configuration.InstructionSet);
        }
    }
}
