using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation
{
    internal class Block
    {
        public List<IntermediateInstruction> Instructions { get; set; } = new List<IntermediateInstruction> { };
    }
}
