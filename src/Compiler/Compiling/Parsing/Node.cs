using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Parsing
{
    internal class Node
    {
        public NodeType Type { get; set; }
        public string Value { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();

        public Node(NodeType type, string value)
        {
            Type = type;
            Value = value;
        }

        public Node(NodeType type)
        {
            Type = type;
        }
    }

    internal enum NodeType
    {
        Assignment,
        Value,
        Variable,
        Program,
        Addition,
        Subtraction,
        Increment,
        Decrement,
        WhileLoop,
        ConditionalStatement,
        Condition,
        Sign,
        Block,
        Input,
        Output,
        Halt,
        Shift
    }
}
