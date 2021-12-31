using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Compiling.Transformation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Translation.Implementations
{
    internal class Translator : ITranslator
    {
        private readonly IInstructionSet _instructionSet;

        public Translator(IInstructionSet instructionSet)
        {
            _instructionSet = instructionSet;
        }

        public string[] Translate(RawInstruction[] instructions)
        {
            var output = new List<string>();

            foreach (var rawInstruction in instructions)
            {
                // Grab Instruction by name
                var instruction = _instructionSet.GetInstruction(rawInstruction.Operation);

                // Unknown Instruction
                if (instruction == null)
                    throw new Exception(string.Format("Translation Error: Couldn't find instruction '{0}' in Instruction Set", rawInstruction.Operation));

                var currentLine = "";

                // OP Code
                currentLine += Convert.ToString(instruction.OPCode, 2).PadLeft(4, '0');

                if (instruction.Translation == null)
                {
                    currentLine += "0000 00000000";
                    output.Add(currentLine);
                    continue;
                }

                var result = instruction.Translation;

                // Parts of translation to replace
                var tokens = Regex.Matches(instruction.Translation, @"(.)\1{1,}")
                    .Where(m => !m.Value.StartsWith("0"))
                    .Select(m => m.Value)
                    .ToArray();

                if (rawInstruction.Operation == Operations.Branch)
                {
                    var condition = _instructionSet.GetCondition((Conditions)rawInstruction.Parameters[0]);

                    // Unknown Condition
                    if (condition == null)
                        throw new Exception(string.Format("Translation Error: Couldn't find condition '{0}' in Instruction Set", rawInstruction.Operation));

                    rawInstruction.Parameters[0] = condition.OPCode;
                }

                for (int i = 0; i < tokens.Count(); i++)
                {
                    // The value
                    var param = (int)rawInstruction.Parameters[int.Parse(tokens[i].First().ToString()) - 1];

                    // Convert value to binary
                    var part = Convert.ToString(param, 2).PadLeft(tokens[i].Length, '0');

                    if (part.Length > tokens[i].Length)
                        throw new Exception(string.Format("Translation Error: Value {0} doesn't fit into the Instruction Translation", param));

                    // Replace part of translation with value
                    result = result.Replace(tokens[i], part);
                }

                // Complete line
                currentLine += result;

                // Add spaces between bytes
                if (currentLine.Length > 8)
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
