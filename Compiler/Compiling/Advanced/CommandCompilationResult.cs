using CompilerTest.Compiling.InstructionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Advanced
{
    internal class CommandCompilationResult
    {
        public bool Success { get; set; }
        public Instruction Instruction { get; set; }
        public int[] Values { get; set; }

        public CommandCompilationResult(bool success, Instruction instruction, int[] values)
        {
            Success = success;
            Instruction = instruction;
            Values = values;
        }

        public CommandCompilationResult(bool success)
        {
            Success = success;
        }

        public CommandCompilationResult()
        {

        }
    }
}
