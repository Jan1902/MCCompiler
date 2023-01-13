namespace MCCompiler.Compiler.Shared;

public class ASTNode
{
    public NodeType Type { get; init; }
    public string? Value { get; init; }
    public List<ASTNode> Children { get; init; }

    public ASTNode(NodeType type, string value)
    {
        Type = type;
        Value = value;
        Children = new();
    }

    public ASTNode(NodeType type)
    {
        Type = type;
        Children = new();
    }
}

public enum NodeType
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
