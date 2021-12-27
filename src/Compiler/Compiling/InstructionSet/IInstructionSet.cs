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
        Instruction GetInstructionByName(string name);
    }
}
