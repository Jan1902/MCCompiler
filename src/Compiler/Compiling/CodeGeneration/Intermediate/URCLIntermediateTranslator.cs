using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.Environment.Models;
using CompilerTest.Compiling.Transformation.Enums;
using CompilerTest.Compiling.Transformation.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.CodeGeneration.Intermediate
{
    internal class URCLIntermediateTranslator : IIntermediateTranslator
    {
        private readonly CompilationEnvironment _environment;

        public URCLIntermediateTranslator(CompilationEnvironment environment)
        {
            _environment = environment;
        }

        public string[] Translate(List<IntermediateInstruction> instructions)
        {
            for (int i = 0; i < _environment.CustomVariables.Count; i++)
            {
                _environment.CustomVariables[i].Name = "$" + (i + 1);
            }

            var output = new List<string>();

            foreach (var instruction in instructions)
            {
                if (instruction.Operation == Operations.LBL)
                {
                    output.Add("." + instruction.Parameters[0].ToString());
                    continue;
                }

                for (int i = 0; i < instruction.Parameters.Length; i++)
                {
                    if (instruction.Parameters[i] is Variable var)
                        instruction.Parameters[i] = var.Name;
                }

                if (instruction.Operation.ToString().StartsWith("B") || instruction.Operation == Operations.JMP)
                    instruction.Parameters[0] = "." + instruction.Parameters[0];

                output.Add(instruction.Operation.ToString() + " " + string.Join(", ", instruction.Parameters));
            }

            return output.ToArray();
        }
    }
}
