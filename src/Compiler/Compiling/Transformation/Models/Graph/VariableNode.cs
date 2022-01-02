using CompilerTest.Compiling.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation.Models.Graph
{
    internal class VariableNode
    {
        public Variable Variable { get; set; }
        public List<VariableNode> Connected { get; set; }
        public int Color { get; set; }

        public VariableNode(Variable variable)
        {
            Connected = new List<VariableNode>();
            Variable = variable;
        }
    }
}
