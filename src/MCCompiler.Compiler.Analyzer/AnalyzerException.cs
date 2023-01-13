using MCCompiler.Compiler.Shared;

namespace MCCompiler.Compiler.Analyzer;

internal class AnalyzerException : Exception
{
    public ASTNode Node { get; set; }

    public AnalyzerException(ASTNode node, string message) : base(message)
    {
        Node = node;
    }
}
