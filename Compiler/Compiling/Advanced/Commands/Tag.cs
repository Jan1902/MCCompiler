using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Other.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Advanced.Commands
{
    internal class Tag : Command
    {
        public override Regex Pattern => new(@"\:\w+$");

        public override bool RequiresNextLine => false;

        public Tag(ILogger logger, IInstructionSet instructionSet) : base(logger, instructionSet) { }

        public override CommandCompilationResult Compile(string line, int lineNr, int instructionNr, CompilationEnvironment environment, string nextLine = null)
        {
            // Grab parts
            var name = line[1..].ToLower();

            if(environment.Tags.Any(t => t.Key == name))
            {
                _logger.LogError("Already used tag '{0}' on line {1}", name, lineNr + 1);
                return new CommandCompilationResult(false);
            }

            environment.Tags.Add(name, instructionNr);

            return new CommandCompilationResult(true, null, null);
        }
    }
}
