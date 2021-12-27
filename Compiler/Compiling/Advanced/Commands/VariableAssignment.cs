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
    internal class VariableAssignment : Command
    {
        public override Regex Pattern => new(@"\w+\=\d+$");

        public override bool RequiresNextLine => false;

        public VariableAssignment(ILogger logger, IInstructionSet instructionSet) : base(logger, instructionSet) { }

        public override CommandCompilationResult Compile(string line, int lineNr, int instructionNr, CompilationEnvironment environment, string nextLine = null)
        {
            // Grab parts
            var variableName = line.Split('=').First().ToLower();
            var value = line.Split('=').Last().ToLower();

            // Find variable
            var variable = environment.CustomVariables.FirstOrDefault(v => v.Name == variableName);

            if (variable == null)
            {
                // Create variable if not fonund
                variable = new Variable(variableName,
                    false,
                    environment.CustomVariables.Any()
                        ? environment.CustomVariables.Max(v => v.Address) + 1
                        : 0);

                environment.CustomVariables.Add(variable);
            }

            return new CommandCompilationResult(true, _instructionSet.GetInstructionByName("LDI"), new int[] { variable.Address, int.Parse(value) });
        }
    }
}
