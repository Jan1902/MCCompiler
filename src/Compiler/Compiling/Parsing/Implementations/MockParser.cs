﻿using CompilerTest.Compiling.Parsing.Models;
using CompilerTest.Compiling.Tokenizing.Models;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Parsing.Implementations
{
    internal class MockParser : IParser
    {
        public ASTNode Parse(Token[] tokens)
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
}
