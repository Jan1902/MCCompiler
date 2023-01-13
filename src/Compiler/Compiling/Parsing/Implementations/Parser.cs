using CompilerTest.Compiling.Parsing.Models;
using CompilerTest.Compiling.Tokenizing.Models;
using System;
using System.Linq;

namespace CompilerTest.Compiling.Parsing.Implementations;

internal class Parser : IParser
{
    private int current;
    private Token token;

    private ASTNode ast;

    private Token[] _tokens;

    private Exception Error(Token token, string message, params object[] values)
    {
        return new ParserException(token, string.Format("Parsing error on line {0}: {1}", token.Line, string.Format(message, values)));
    }

    private Exception Error(string message, params object[] values)
    {
        return new ParserException(token, string.Format("Parsing error: {1}", token.Line, string.Format(message, values)));
    }

    private bool PeekType(int count, params TokenType[] types)
    {
        return _tokens.Length > current + count
            && types.Contains(_tokens[current + count].Type);
    }

    private void Consume(TokenType type, bool advance = true)
    {
        if (token.Type != type)
            throw Error(token, "Expected '{0}', got '{1}' instead", type, token.Content);
        if (advance)
            Advance();
    }

    private void Advance(int count = 1)
    {
        current += count;

        if (current == _tokens.Length)
            return;

        token = _tokens[current];
    }

    private void CheckVariable()
    {
        if (!ast.Children.Any(c => c.Type == NodeType.Assignment && c.Children[0].Value == token.Content))
            throw Error(token, "Used unassigned variable '{0}'", token.Content);
    }

    public ASTNode Parse(Token[] tokens)
    {
        _tokens = tokens;
        token = _tokens[0];

        ast = new ASTNode(NodeType.Root, null);

        while (current < _tokens.Length)
            ast.Children.Add(Walk());

        return ast;
    }

    private ASTNode Walk()
    {
        // Increment
        if (token.Type == TokenType.Identifier
            && PeekType(1, TokenType.Plus)
            && PeekType(2, TokenType.Plus))
        {
            var variable = token.Content;
            CheckVariable();
            Advance(3);
            return new ASTNode(NodeType.Increment, variable);
        }

        // Decrement
        if (token.Type == TokenType.Identifier
            && PeekType(1, TokenType.Minus)
            && PeekType(2, TokenType.Minus))
        {
            var variable = token.Content;
            CheckVariable();
            Advance(3);
            return new ASTNode(NodeType.Decrement, variable);
        }

        // Condition
        if (token.Type == TokenType.Identifier
            && (PeekType(1, TokenType.Smaller, TokenType.Bigger)
                && PeekType(2, TokenType.Number, TokenType.Identifier)
            || PeekType(1, TokenType.Smaller, TokenType.Bigger, TokenType.Equals)
                && PeekType(2, TokenType.Equals)
                && PeekType(3, TokenType.Number, TokenType.Identifier)))
        {
            var node = new ASTNode(NodeType.Condition);

            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
            CheckVariable();
            Advance();

            var sign = token.Content;
            Advance();
            if (token.Type == TokenType.Equals)
            {
                sign += token.Content;
                Advance();
            }

            node.Children.Add(new ASTNode(NodeType.Sign, sign));

            if (token.Type == TokenType.Identifier)
            {
                CheckVariable();
                node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
            }
            else
            {
                node.Children.Add(new ASTNode(NodeType.Value, token.Content));
            }
            Advance();

            return node;
        }

        // Constant assignment
        if(token.Type == TokenType.KeyWord && token.Content == "const")
        {
            Advance();
            Consume(TokenType.Identifier, false);
            var name = token.Content;
            Advance();

            Consume(TokenType.Equals);

            Consume(TokenType.Number, false);
            var value = token.Content;

            Advance();

            var node = new ASTNode(NodeType.ConstantAssignment);
            node.Children.Add(new ASTNode(NodeType.Identifier, name));
            node.Children.Add(new ASTNode(NodeType.Value, value));

            return node;
        }

        // Assignment
        if (token.Type == TokenType.Identifier
            && PeekType(1, TokenType.Equals)
            && PeekType(2, TokenType.Number, TokenType.Identifier, TokenType.KeyWord))
        {
            var node = new ASTNode(NodeType.Assignment);

            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
            Advance(2);
            node.Children.Add(Walk());

            return node;
        }

        // Arithmetic
        if (token.Type == TokenType.Identifier
            && PeekType(1, TokenType.Plus, TokenType.Minus, TokenType.Asterisc, TokenType.Slash, TokenType.Percent)
            && PeekType(2, TokenType.Identifier, TokenType.Number))
        {
            CheckVariable();

            var node = new ASTNode(NodeType.Arithmetic);

            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
            Advance();
            node.Children.Add(new ASTNode(NodeType.Sign, token.Content));
            Advance();
            node.Children.Add(Walk());

            return node;
        }

        // Conditional Statement
        if (token.Type == TokenType.KeyWord && token.Content == "if")
        {
            Advance();
            // Opening Bracket for condition
            Consume(TokenType.LeftBracket);

            var node = new ASTNode(NodeType.ConditionalStatement);

            // Condition
            node.Children.Add(Walk());

            // Closing Bracket for condition
            Consume(TokenType.RightBracket);

            // Opening Bracket for block
            Consume(TokenType.LeftCurlyBracket);
            var blockStartLine = token.Line;

            // Block
            var block = new ASTNode(NodeType.Block);

            // Walk till closing bracket for block
            while (token.Type != TokenType.RightCurlyBracket)
                block.Children.Add(Walk());

            if (token.Type != TokenType.RightCurlyBracket)
                throw Error("No closing bracket found for block starting at line {0}", blockStartLine);

            node.Children.Add(block);

            // Closing Bracket for block
            Consume(TokenType.RightCurlyBracket);

            return node;
        }

        // While loop
        if (token.Type == TokenType.KeyWord && token.Content == "while")
        {
            Advance();
            // Opening Bracket for condition
            Consume(TokenType.LeftBracket);

            var node = new ASTNode(NodeType.WhileLoop);

            // Condition
            node.Children.Add(Walk());

            // Closing Bracket for condition
            Consume(TokenType.RightBracket);

            // Opening Bracket for block
            Consume(TokenType.LeftCurlyBracket);
            var blockStartLine = token.Line;

            // Block
            var block = new ASTNode(NodeType.Block);

            // Walk till closing bracket for block
            while (token.Type != TokenType.RightCurlyBracket)
                block.Children.Add(Walk());

            if (token.Type != TokenType.RightCurlyBracket)
                throw Error("No closing bracket found for block starting at line {0}", blockStartLine);

            node.Children.Add(block);

            // Closing Bracket for block
            Consume(TokenType.RightCurlyBracket);

            return node;
        }

        // Halt
        if (token.Type == TokenType.KeyWord && token.Content == "halt")
        {
            Advance();
            Consume(TokenType.LeftBracket);
            Consume(TokenType.RightBracket);
            return new ASTNode(NodeType.Halt);
        }

        // Input
        if (token.Type == TokenType.KeyWord && token.Content == "input")
        {
            Advance();
            Consume(TokenType.LeftBracket);
            Consume(TokenType.Number, false);

            var node = new ASTNode(NodeType.Input);
            node.Children.Add(new ASTNode(NodeType.Value, token.Content));

            Advance();
            Consume(TokenType.RightBracket);
            return node;
        }

        // Output
        if (token.Type == TokenType.KeyWord && token.Content == "output")
        {
            Advance();
            Consume(TokenType.LeftBracket);

            var node = new ASTNode(NodeType.Output);

            Consume(TokenType.Number, false);
            node.Children.Add(new ASTNode(NodeType.Value, token.Content));

            Advance();
            Consume(TokenType.Comma);

            Consume(TokenType.Identifier, false);
            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));

            Advance();
            Consume(TokenType.RightBracket);
            return node;
        }

        // Shift
        if (token.Type == TokenType.Identifier
            && PeekType(1, TokenType.Bigger, TokenType.Smaller)
            && PeekType(2, TokenType.Bigger, TokenType.Smaller))
        {
            var node = new ASTNode(NodeType.Shift);

            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
            Advance();

            var type = token.Type;
            node.Children.Add(new ASTNode(NodeType.Sign, token.Content));

            Advance();

            // Make sure its the same sign twice
            Consume(type);

            return node;
        }

        // Function
        if(token.Type == TokenType.KeyWord && token.Content == "func")
        {
            var node = new ASTNode(NodeType.FunctionDefinition);
            Advance();
            Consume(TokenType.Identifier, false);

            // Function Name
            node.Children.Add(new ASTNode(NodeType.Value, token.Content));

            Advance();

            // No parameters yet
            Consume(TokenType.LeftBracket);
            Consume(TokenType.RightBracket);

            // Opening Curly Bracket
            Consume(TokenType.LeftCurlyBracket);

            var blockStartLine = token.Line;

            // Block
            var block = new ASTNode(NodeType.Block);

            // Walk till closing bracket for block
            while (token.Type != TokenType.RightCurlyBracket)
                block.Children.Add(Walk());

            if (token.Type != TokenType.RightCurlyBracket)
                throw Error("No closing bracket found for block starting at line {0}", blockStartLine);

            node.Children.Add(block);

            // Closing Bracket for block
            Consume(TokenType.RightCurlyBracket);

            return node;
        }

        // Number
        if (token.Type == TokenType.Number)
        {
            var node = new ASTNode(NodeType.Value, token.Content);
            Advance();
            return node;
        }

        // Variable
        if (token.Type == TokenType.Identifier)
        {
            var node = new ASTNode(NodeType.Identifier, token.Content);
            Advance();
            return node;
        }

        throw Error(token, "Unknown token '{0}'", token.Content);
    }
}
