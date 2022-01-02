using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation
{
    internal class IntermediateInstruction
    {
        public Operations Operation { get; set; }
        public object[] Parameters { get; set; }

        public IntermediateInstruction(Operations operation, params object[] parameters)
        {
            Operation = operation;
            Parameters = parameters;
        }

        public IntermediateInstruction()
        {

        }
    }
}
