using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Other.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Advanced
{
    internal class Command
    {
        public virtual Regex Pattern { get; }
        public virtual bool RequiresNextLine { get; }

        protected readonly ILogger _logger;
        protected readonly IInstructionSet _instructionSet;

        public Command(ILogger logger, IInstructionSet instructionSet)
        {
            _logger = logger;
            _instructionSet = instructionSet;
        }

        public virtual CommandCompilationResult Compile(string line,
            int lineNr,
            int instructionNr,
            CompilationEnvironment environment,
            string nextLine = null)
        {
            throw new NotImplementedException();
        }
    }
}
