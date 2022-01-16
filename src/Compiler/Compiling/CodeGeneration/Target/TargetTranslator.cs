using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Compiling.Transformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CompilerTest.Compiling.CodeGeneration.Target
{
    internal class TargetTranslator : ITargetTranslator
    {
        private readonly BasicInstructionSet _instructionSet;

        public TargetTranslator(BasicInstructionSet instructionSet)
        {
            _instructionSet = instructionSet;
        }

        public string[] Translate(List<IntermediateInstruction> instructions)
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

                var result = instruction.Translation.ToLower();

                // Parts of translation to replace
                var tokens = Regex.Matches(result, @"([a-z])\1{1,}")
                    .Select(m => m.Value)
                    .ToArray();

                for (int i = 0; i < tokens.Count(); i++)
                {
                    // The value
                    var param = 0;
                    if(rawInstruction.Parameters.Length > tokens[i].First() - 97)
                        param = int.Parse(rawInstruction.Parameters[tokens[i].First() - 97].ToString());

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
