using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.Transformation.Enums;
using CompilerTest.Compiling.Transformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilerTest.Compiling.CodeGeneration.Target.IntermediateParsing
{
    internal class URCLIntermediateParser : IIntermediateParser
    {
        private readonly CompilationEnvironment _compilationEnvironment;

        public URCLIntermediateParser(CompilationEnvironment compilationEnvironment)
        {
            _compilationEnvironment = compilationEnvironment;
        }

        public List<IntermediateInstruction> Parse(string[] input)
        {
            var instructions = new List<IntermediateInstruction>();
            foreach (var line in input)
            {
                var instruction = new IntermediateInstruction();
                var parts = line.Replace(",", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);

                instruction.Operation = (Operations)Enum.Parse(typeof(Operations), parts[0]);

                var parameters = new List<object>();
                foreach (var param in parts.Skip(1))
                {
                    if(param.StartsWith("$"))
                    {
                        var variable = _compilationEnvironment.GetOrCreateVariable(param[1..]);
                        parameters.Add(variable);
                        continue;
                    }

                    parameters.Add(param);
                }

                instruction.Parameters = parameters.ToArray();

                instructions.Add(instruction);
            }

            return instructions;
        }
    }
}
