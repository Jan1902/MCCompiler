using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Parsing
{
    internal class MockParser : IParser
    {
        public Node Parse()
        {
            var ast = new Node(NodeType.Program);

            ast.Children.Add(new Node(NodeType.Assignment)
            {
                Children = new List<Node>()
                {
                    new Node(NodeType.Variable, "a"),
                    new Node(NodeType.Value, "1")
                }
            });

            ast.Children.Add(new Node(NodeType.Assignment)
            {
                Children = new List<Node>()
                {
                    new Node(NodeType.Variable, "b"),
                    new Node(NodeType.Value, "2")
                }
            });

            ast.Children.Add(new Node(NodeType.Assignment)
            {
                Children = new List<Node>()
                {
                    new Node(NodeType.Variable, "c"),
                    new Node(NodeType.Addition)
                    {
                        Children = new List<Node>()
                        {
                            new Node(NodeType.Variable, "a"),
                            new Node(NodeType.Variable, "b")
                        }
                    }
                }
            });

            return ast;
        }
    }
}
