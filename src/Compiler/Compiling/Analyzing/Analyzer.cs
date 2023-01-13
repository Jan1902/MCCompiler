using CompilerTest.Compiling.Parsing.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Analyzing;

internal class Analyzer : IAnalyzer
{
    private List<NodeType> validTopLevelNodes = new List<NodeType>()
    {
        NodeType.Assignment,
        NodeType.ConstantAssignment,
        NodeType.Output,
        NodeType.ConditionalStatement,
        NodeType.Halt,
        NodeType.WhileLoop
    };

    private List<NodeType> validAssignments = new List<NodeType>()
    {
        NodeType.Arithmetic,
        NodeType.Increment,
        NodeType.Decrement,
        NodeType.Input,
        NodeType.Identifier,
        NodeType.Value,
        NodeType.Shift
    };

    private AnalyzerException Error(ASTNode node, string message, params object[] objects)
    {
        return new AnalyzerException(node, "Analyzer Error: " + string.Format(message, objects));
    }

    public ASTNode AnalyzeAndCleanUp(ASTNode ast)
    {
        var toRemove = new List<ASTNode>();

        foreach (var node in ast.Children)
        {
            // Unnecessary Top Level Nodes
            if (!validTopLevelNodes.Contains(node.Type))
                toRemove.Add(node);

            // Invalid Assignments
            if (node.Type == NodeType.Assignment && !validAssignments.Contains(node.Children[1].Type))
                throw Error(node, "Invalid assignment of {0} to {1}", node.Children[1].Type, node.Children[0].Type);
        }

        foreach (var node in toRemove)
        {
            ast.Children.Remove(node);
        }

        return ast;
    }
}
