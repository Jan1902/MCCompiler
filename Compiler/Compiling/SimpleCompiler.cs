using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Other;
using CompilerTest.Other.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling
{
    public class SimpleCompiler : ICompiler
    {
        private readonly IInstructionSet _instructionSet;
        private readonly ILogger _logger;

        public SimpleCompiler(IInstructionSet instructionSet, ILogger logger)
        {
            _instructionSet = instructionSet;
            _logger = logger;
        }

        public string[] Compile(string source)
        {
            var output = new List<string>();

            var lines = source.SplitLines();

            for (int l = 0; l < lines.Length; l++)
            {
                var line = lines[l];

                // Ignore comments
                if (line.StartsWith("//") || string.IsNullOrWhiteSpace(line))
                    continue;

                // Characters to replace
                var replaceDict = new Dictionary<string, string>()
                {
                    { ", ", " " },
                    { ",", "" },
                    { "_", "0" }
                };

                // Convert from human readable
                var parts = line
                    .ReplaceMany(replaceDict)
                    .Split(" ");

                // Grab Instruction by name
                var instruction = _instructionSet.GetInstructionByName(parts[0]);

                // Unknown Instruction
                if (instruction == null)
                {
                    _logger.LogWarning("Found unknown instruction '{0}' in line {1}", parts[0], l + 1);
                    continue;
                }

                var currentLine = "";

                // OP Code
                currentLine += Convert.ToString(instruction.OPCode, 2).PadLeft(4, '0');

                var result = instruction.Translation;

                // Parts of translation to replace
                var tokens = Regex.Matches(instruction.Translation, @"(.)\1{1,}")
                    .Where(m => !m.Value.StartsWith("0"))
                    .Select(m => m.Value)
                    .ToArray();

                for (int i = 0; i < tokens.Count(); i++)
                {
                    // The value
                    var part = parts[int.Parse(tokens[i].First().ToString())];

                    // Register Definition ignored here
                    if (part.StartsWith("$"))
                        part = part[1..];

                    // Convert value to binary
                    part = Convert.ToString(int.Parse(part), 2).PadLeft(tokens[i].Length, '0');

                    // Replace part of translation with value
                    result = result.Replace(tokens[i], part);
                }

                // Complete line
                currentLine += result;

                // Add spaces between bytes
                if(currentLine.Length > 8)
                {
                    for (int i = 0; i < currentLine.Length / 8 - 1; i++)
                        currentLine = currentLine.Substring(i * 8, 8) + " " + currentLine.Substring((i + 1) * 8);
                }

                output.Add(currentLine);
            }

            return output.ToArray();
        }
    }
}
