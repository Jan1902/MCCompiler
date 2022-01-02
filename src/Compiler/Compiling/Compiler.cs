﻿using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Compiling.Parsing;
using CompilerTest.Compiling.Parsing.Implementations;
using CompilerTest.Compiling.Tokenizing;
using CompilerTest.Compiling.Tokenizing.Implementations;
using CompilerTest.Compiling.Transformation;
using CompilerTest.Compiling.Transformation.Implementations;
using CompilerTest.Compiling.Translation;
using CompilerTest.Compiling.Translation.Implementations;
using CompilerTest.Components;
using CompilerTest.Configuration;
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
        private readonly IComponentProvider _componentProvider;

        public Compiler(IComponentProvider componentProvider)
        {
            _componentProvider = componentProvider;
        }

        public string[] Compile(string code)
        {
            var environment = new CompilationEnvironment();

            ITokenizer tokenizer = _componentProvider.Tokenizer;
            var tokens = tokenizer.Tokenize(code);

            IParser parser = _componentProvider.Parser;
            var ast = parser.Parse(tokens);

            ITransformer transformer = _componentProvider.Transformer;
            var instructions = transformer.Transform(ast);

            var registerAllocator = new RegisterAllocator();
            instructions = registerAllocator.AllocateRegisters(instructions).ToList();

            ITranslator translator = _componentProvider.Translator;
            var output = translator.Translate(instructions);

            return output.ToArray();
        }
    }
}
