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
    internal class Addition : Command
    {
        public override Regex Pattern => new(@"\w=\w\+\w$");

        public override bool RequiresNextLine => false;

        public Addition(ILogger logger, IInstructionSet instructionSet) : base(logger, instructionSet) { }

        public override CommandCompilationResult Compile(string line, int lineNr, int instructionNr, CompilationEnvironment environment, string nextLine = null)
        {
            // Grab parts
            var dst = line.Split('=').First();
            var a = line.Split('=').Last().Split("+").First();
            var b = line.Split('=').Last().Split("+").Last();

            // Find variables
            var dstVariable = environment.CustomVariables.FirstOrDefault(v => v.Name == dst);
            var aVariable = environment.CustomVariables.FirstOrDefault(v => v.Name == a);
            var bVariable = environment.CustomVariables.FirstOrDefault(v => v.Name == b);

            // Check for undefined variables
            if(aVariable == null)
            {
                _logger.LogError("Undefined variable '{0}' on line {1}", a, lineNr + 1);
                return new CommandCompilationResult(false);
            }

            if (bVariable == null)
            {
                _logger.LogError("Undefined variable '{0}' on line {1}", b, lineNr + 1);
                return new CommandCompilationResult(false);
            }

            if (dstVariable == null)
            {
                // Create destination variable if not found
                dstVariable = new Variable(dst,
                    false,
                    environment.CustomVariables.Any()
                        ? environment.CustomVariables.Max(v => v.Address) + 1
                        : 0);

                environment.CustomVariables.Add(dstVariable);
            }

            return new CommandCompilationResult(true, _instructionSet.GetInstructionByName("ADD"), new int[] { dstVariable.Address, aVariable.Address, bVariable.Address });
        }
    }
}
