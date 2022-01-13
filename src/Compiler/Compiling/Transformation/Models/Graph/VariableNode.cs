using CompilerTest.Compiling.Environment.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Transformation.Models.Graph
{
    internal class VariableNode
    {
        public Variable Variable { get; set; }
        public List<VariableNode> Connected { get; set; }
        public List<VariableNode> OriginallyConnected { get; set; }
        public int Color { get; set; }

        public VariableNode(Variable variable)
        {
            Connected = new List<VariableNode>();
            Variable = variable;
        }
    }
}
