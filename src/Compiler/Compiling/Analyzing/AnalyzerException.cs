using CompilerTest.Compiling.Parsing.Models;
using System;

namespace CompilerTest.Compiling.Analyzing;

internal class AnalyzerException : Exception
{
    public ASTNode Node { get; set; }

    public AnalyzerException(ASTNode node, string message) : base(message)
    {
        Node = node;
    }
}
