using CompilerTest.Compiling.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    public interface IInstructionSet
    {
        ICollection<Instruction> Instructions { get; set; }
        ICollection<Condition> Conditions { get; set; }
        Instruction GetInstruction(Operations name);
        Condition GetCondition(Conditions name);
    }
}
