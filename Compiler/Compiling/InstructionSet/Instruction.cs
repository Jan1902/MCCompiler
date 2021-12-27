using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    public class Instruction
    {
        public string Translation { get; set; }
        public string Name { get; set; }
        public int OPCode { get; set; }

        public Instruction(string translation, string name, int oPCode)
        {
            Translation = translation;
            Name = name;
            OPCode = oPCode;
        }

        public Instruction()
        {

        }
    }
}
