using MCCompiler.Compiler.Abstraction;
using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Parser.Implementations;

public class MockParser : ParserBase, IParser
{
    public ASTNode Parse(IEnumerable<Token> _)
    {
        var ast = new ASTNode(NodeType.Root);

        ast.Children.Add(new ASTNode(NodeType.Assignment)
        {
            Children = new List<ASTNode>()
            {
                new ASTNode(NodeType.Identifier, "a"),
                new ASTNode(NodeType.Value, "1")
            }
        });

        ast.Children.Add(new ASTNode(NodeType.Assignment)
        {
            Children = new List<ASTNode>()
            {
                new ASTNode(NodeType.Identifier, "b"),
                new ASTNode(NodeType.Value, "2")
            }
        });

        ast.Children.Add(new ASTNode(NodeType.Assignment)
        {
            Children = new List<ASTNode>()
            {
                new ASTNode(NodeType.Identifier, "c"),
                new ASTNode(NodeType.Arithmetic)
                {
                    Children = new List<ASTNode>()
                    {
                        new ASTNode(NodeType.Identifier, "a"),
                        new ASTNode(NodeType.Sign, "+"),
                        new ASTNode(NodeType.Identifier, "b")
                    }
                }
            }
        });

        return ast;
    }
}
