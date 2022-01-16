using System.Collections.Generic;

namespace CompilerTest.Compiling.Parsing.Models
{
    internal class ASTNode
    {
        public NodeType Type { get; set; }
        public string Value { get; set; }
        public List<ASTNode> Children { get; set; } = new List<ASTNode>();

        public ASTNode(NodeType type, string value)
        {
            Type = type;
            Value = value;
        }

        public ASTNode(NodeType type)
        {
            Type = type;
        }
    }

    internal enum NodeType
    {
        Assignment,
        ConstantAssignment,
        Value,
        Identifier,
        Root,
        Arithmetic,
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
        Shift,
        FunctionDefinition,
        FunctionCall
    }
}
