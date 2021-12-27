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
    internal class Jump : Command
    {
        public override Regex Pattern => new(@"jump\(\w+\)$");

        public override bool RequiresNextLine => false;

        public Jump(ILogger logger, IInstructionSet instructionSet) : base(logger, instructionSet) { }

        public override CommandCompilationResult Compile(string line, int lineNr, int instructionNr, CompilationEnvironment environment, string nextLine = null)
        {
            // Grab tag name
            var tagName = line.Substring(5, line.Length - 6).ToLower();

            if (!environment.Tags.ContainsKey(tagName))
            {
                _logger.LogError("Undefined tag '{0}' on line {1}", tagName, lineNr + 1);
                return new CommandCompilationResult(false);
            }

            // Find tag
            var tag = environment.Tags.FirstOrDefault(t => t.Key == tagName);

            return new CommandCompilationResult(true, _instructionSet.GetInstructionByName("JC"), new int[] { 0, tag.Value });
        }
    }
}
