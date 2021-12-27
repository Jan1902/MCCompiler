using CompilerTest.Compiling.Advanced.Commands;
using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Other;
using CompilerTest.Other.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Advanced
{
    internal class AdvancedCompiler : ICompiler
    {
        private readonly ILogger _logger;
        private readonly IInstructionSet _instructionSet;

        private List<Command> commands;

        public AdvancedCompiler(ILogger logger, IInstructionSet instructionSet)
        {
            _instructionSet = instructionSet;
            _logger = logger;

            // Available Commands
            commands = new List<Command>();
            commands.Add(new VariableAssignment(_logger, _instructionSet));
            commands.Add(new Addition(_logger, _instructionSet));
            commands.Add(new Tag(_logger, _instructionSet));
            commands.Add(new Jump(_logger, _instructionSet));
        }

        public string[] Compile(string code)
        {
            var output = new List<string>();
            var environment = new CompilationEnvironment();

            // Split lines
            var lines = code.SplitLines().Select(l => l.Trim()).ToArray();

            // Iterate lines
            for (int i = 0; i < lines.Length; i++)
            {
                // Skip comments and empty lines
                if (lines[i].StartsWith("//") || string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                // Remove spaces
                var line = lines[i].Replace(" ", "");

                // Find matching command
                var command = commands.FirstOrDefault(c => c.Pattern.IsMatch(line));

                // Handle unknown command
                if(command == null)
                {
                    _logger.LogWarning("Found unknown command on line {0}: {1}", i + 1, lines[i]);
                    continue;
                }

                // Compile command
                var result = command.Compile(line, i, output.Count, environment, command.RequiresNextLine && lines.Length > i + 1 ? lines[i + 1].Replace(" ", "") : null);

                // Stop if unsuccessful
                if (!result.Success)
                {
                    _logger.LogError("Error compiling line {0}. Stopping...", i + 1);
                    return null;
                }

                // Command without generated code
                if (result.Instruction == null)
                    continue;

                // Either start with translation pattern or zeros
                var generatedLine = result.Values == null ? "000000000000" : result.Instruction.Translation;

                // Might be an instruction without data
                if(result.Values != null)
                {
                    // Tokenize translation
                    var tokens = Regex.Matches(result.Instruction.Translation, @"(.)\1{1,}")
                        .Where(m => !m.Value.StartsWith("0"))
                        .Select(m => m.Value)
                        .ToArray();

                    // Iterate Tokens
                    for (int x = 0; x < tokens.Count(); x++)
                    {
                        // Generate translation
                        var part = result.Values[int.Parse(tokens[x].First().ToString()) - 1];
                        var partTranslation = Convert.ToString(part, 2).PadLeft(tokens[x].Length, '0');

                        generatedLine = generatedLine.Replace(tokens[x], partTranslation);
                    }
                }

                // Add OP Code
                generatedLine = Convert.ToString(result.Instruction.OPCode, 2).PadLeft(4, '0') + generatedLine;

                // Add spaces between bytes
                if (generatedLine.Length > 8)
                {
                    for (int z = 0; z < generatedLine.Length / 8 - 1; z++)
                        generatedLine = generatedLine.Substring(z * 8, 8) + " " + generatedLine.Substring((z + 1) * 8);
                }

                output.Add(generatedLine);
            }

            return output.ToArray();
        }
    }
}
