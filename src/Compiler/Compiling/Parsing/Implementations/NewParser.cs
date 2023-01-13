using CompilerTest.Compiling.Parsing.Models;
using CompilerTest.Compiling.Tokenizing.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilerTest.Compiling.Parsing.Implementations;

internal class NewParser : IParser
{
    private int current;
    private Token token;
    private Token[] _tokens;

    private ASTNode ast;

    private readonly Dictionary<CheckOperation, ParseOperation> parsers;
    private readonly Dictionary<string, ParseOperation> functionParsers;
    private readonly Dictionary<string, ParseOperation> loopParsers;

    private delegate bool CheckOperation();
    private delegate ASTNode ParseOperation();

    public NewParser()
    {
        parsers = new Dictionary<CheckOperation, ParseOperation>()
        {
            { CheckAssignment, ParseAssignment },
            { CheckConstantAssignment, ParseConstantAssignment },
            { CheckArithmetic, ParseArithmetic },
            { CheckIncrementDecrement, ParseIncrementDecrement },
            { CheckShift, ParseShift },
            { CheckLoop, ParseLoop },
            { CheckConditionalStatement, ParseConditionalStatement },
            { CheckCondition, ParseCondition },
            { CheckFunctionCall, ParseFunctionCall },
            { CheckFunctionDefinition, ParseFunctionDefinition },
            { CheckPrimitive, ParsePrimitive }
        };

        functionParsers = new Dictionary<string, ParseOperation>()
        {
            { "halt", ParseHalt },
            { "input", ParseInput },
            { "output", ParseOutput }
        };

        loopParsers = new Dictionary<string, ParseOperation>()
        {
            { "while", ParseWhileLoop }
        };
    }

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

    private void Consume(params TokenType[] types)
    {
        if (!types.Contains(token.Type))
            throw Error(token, "Expected '{0}', got '{1}' instead", string.Join(" or ", types), token.Content);

        Advance();
    }

    private void Expect(params TokenType[] types)
    {
        if (!types.Contains(token.Type))
            throw Error(token, "Expected '{0}', got '{1}' instead", string.Join(" or ", types), token.Content);
    }

    private void Advance(int count = 1)
    {
        current += count;

        if (current == _tokens.Length)
            return;

        token = _tokens[current];
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
        foreach (var operation in parsers.Keys)
        {
            if (operation())
                return parsers[operation]();
        }

        throw Error(token, "Unexpected token '{0}'", token.Content);
    }

    private bool CheckAssignment()
    {
        return PeekType(0, TokenType.Identifier)
            && PeekType(1, TokenType.Equals)
            && PeekType(2, TokenType.Number, TokenType.Identifier, TokenType.KeyWord);
    }

    private ASTNode ParseAssignment()
    {
        var node = new ASTNode(NodeType.Assignment);

        node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
        Advance(2);
        node.Children.Add(Walk());

        return node;
    }

    private bool CheckConstantAssignment()
    {
        return PeekType(0, TokenType.KeyWord)
            && token.Content == "const"
            && PeekType(1, TokenType.Identifier)
            && PeekType(2, TokenType.Equals)
            && PeekType(3, TokenType.Number);
    }

    private ASTNode ParseConstantAssignment()
    {
        Advance();
        Expect(TokenType.Identifier);
        var name = token.Content;
        Advance();

        Consume(TokenType.Equals);

        Expect(TokenType.Number);
        var value = token.Content;

        Advance();

        var node = new ASTNode(NodeType.ConstantAssignment);
        node.Children.Add(new ASTNode(NodeType.Identifier, name));
        node.Children.Add(new ASTNode(NodeType.Value, value));

        return node;
    }

    private bool CheckFunctionCall()
    {
        return PeekType(0, TokenType.Identifier, TokenType.KeyWord)
            && PeekType(1, TokenType.LeftBracket);
    }

    private ASTNode ParseFunctionCall()
    {
        ASTNode node = null;
        if (functionParsers.ContainsKey(token.Content))
            node = functionParsers[token.Content]();
        else
        {
            node = new ASTNode(NodeType.FunctionCall);
            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
            Consume(TokenType.LeftBracket);
            while(token.Type != TokenType.RightBracket)
            {
                if (token.Type == TokenType.Comma)
                    Advance();

                Expect(TokenType.Number, TokenType.Identifier);

                node.Children.Add(new ASTNode(token.Type == TokenType.Number ? NodeType.Value : NodeType.Identifier, token.Content));
            }
        }

        return node;
    }

    private ASTNode ParseInput()
    {
        Advance();
        Consume(TokenType.LeftBracket);
        Expect(TokenType.Number);

        var node = new ASTNode(NodeType.Input);
        node.Children.Add(new ASTNode(NodeType.Value, token.Content));

        Advance();
        Consume(TokenType.RightBracket);

        return node;
    }

    private ASTNode ParseOutput()
    {
        Advance();
        Consume(TokenType.LeftBracket);

        var node = new ASTNode(NodeType.Output);

        Expect(TokenType.Number, TokenType.Identifier);
        node.Children.Add(new ASTNode(token.Type == TokenType.Number ? NodeType.Value : NodeType.Identifier, token.Content));

        Advance();
        Consume(TokenType.Comma);

        Expect(TokenType.Identifier);
        node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));

        Advance();
        Consume(TokenType.RightBracket);

        return node;
    }

    private ASTNode ParseHalt()
    {
        Advance();
        Consume(TokenType.LeftBracket);
        Consume(TokenType.RightBracket);

        return new ASTNode(NodeType.Halt);
    }

    private bool CheckIncrementDecrement()
    {
        return PeekType(0, TokenType.Identifier)
            && ((PeekType(1, TokenType.Plus)
                && PeekType(2, TokenType.Plus))
                || (PeekType(1, TokenType.Minus)
                && PeekType(2, TokenType.Minus)));
    }

    private ASTNode ParseIncrementDecrement()
    {
        var variable = token.Content;
        Advance();

        var sign = token.Content;
        Advance(2);

        return new ASTNode(sign == "+" ? NodeType.Increment : NodeType.Decrement, variable);
    }

    private bool CheckCondition()
    {
        return token.Type == TokenType.Identifier
            && (PeekType(1, TokenType.Smaller, TokenType.Bigger)
                && PeekType(2, TokenType.Number, TokenType.Identifier)
            || PeekType(1, TokenType.Smaller, TokenType.Bigger, TokenType.Equals)
                && PeekType(2, TokenType.Equals)
                && PeekType(3, TokenType.Number, TokenType.Identifier));
    }

    private ASTNode ParseCondition()
    {
        var node = new ASTNode(NodeType.Condition);

        node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
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
            node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
        else
            node.Children.Add(new ASTNode(NodeType.Value, token.Content));
        Advance();

        return node;
    }

    private bool CheckArithmetic()
    {
        return token.Type == TokenType.Identifier
            && PeekType(1, TokenType.Plus, TokenType.Minus, TokenType.Asterisc, TokenType.Slash, TokenType.Percent)
            && PeekType(2, TokenType.Identifier, TokenType.Number);
    }

    private ASTNode ParseArithmetic()
    {
        var node = new ASTNode(NodeType.Arithmetic);

        node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
        Advance();
        node.Children.Add(new ASTNode(NodeType.Sign, token.Content));
        Advance();
        node.Children.Add(Walk());

        return node;
    }

    private bool CheckConditionalStatement()
    {
        return token.Type == TokenType.KeyWord && token.Content == "if";
    }

    private ASTNode ParseConditionalStatement()
    {
        Advance();
        Consume(TokenType.LeftBracket);

        var node = new ASTNode(NodeType.ConditionalStatement);

        node.Children.Add(Walk());

        Consume(TokenType.RightBracket);

        Consume(TokenType.LeftCurlyBracket);
        var blockStartLine = token.Line;

        var block = new ASTNode(NodeType.Block);

        while (token.Type != TokenType.RightCurlyBracket)
            block.Children.Add(Walk());

        if (token.Type != TokenType.RightCurlyBracket)
            throw Error("No closing bracket found for block starting at line {0}", blockStartLine);

        node.Children.Add(block);

        Consume(TokenType.RightCurlyBracket);

        return node;
    }

    private bool CheckShift()
    {
        return PeekType(0, TokenType.Identifier)
            && ((PeekType(1, TokenType.Smaller)
                && PeekType(2, TokenType.Smaller))
                || (PeekType(1, TokenType.Bigger)
                && PeekType(2, TokenType.Bigger)));
    }

    private ASTNode ParseShift()
    {
        var node = new ASTNode(NodeType.Shift);

        node.Children.Add(new ASTNode(NodeType.Identifier, token.Content));
        Advance();

        node.Children.Add(new ASTNode(NodeType.Sign, token.Content));
        Advance(2);

        return node;
    }

    private bool CheckLoop()
    {
        return PeekType(0, TokenType.KeyWord) && loopParsers.ContainsKey(token.Content);
    }

    private ASTNode ParseLoop()
    {
        return loopParsers[token.Content]();
    }

    private ASTNode ParseWhileLoop()
    {
        Advance();
        Consume(TokenType.LeftBracket);

        var node = new ASTNode(NodeType.WhileLoop);

        var condition = Walk();
        if (condition.Type != NodeType.Condition)
            throw Error(token, "Invalid Condition provided");

        node.Children.Add(condition);

        Consume(TokenType.RightBracket);

        Consume(TokenType.LeftCurlyBracket);
        var blockStartLine = token.Line;

        var block = new ASTNode(NodeType.Block);

        while (token.Type != TokenType.RightCurlyBracket)
            block.Children.Add(Walk());

        if (token.Type != TokenType.RightCurlyBracket)
            throw Error("No closing bracket found for block starting at line {0}", blockStartLine);

        node.Children.Add(block);

        Consume(TokenType.RightCurlyBracket);

        return node;
    }

    private bool CheckFunctionDefinition()
    {
        return token.Type == TokenType.KeyWord && token.Content == "func";
    }

    private ASTNode ParseFunctionDefinition()
    {
        var node = new ASTNode(NodeType.FunctionDefinition);
        Advance();
        Expect(TokenType.Identifier);

        node.Children.Add(new ASTNode(NodeType.Value, token.Content));

        Advance();

        //TODO: Implement parameters
        Consume(TokenType.LeftBracket);
        Consume(TokenType.RightBracket);

        Consume(TokenType.LeftCurlyBracket);

        var blockStartLine = token.Line;

        var block = new ASTNode(NodeType.Block);

        while (token.Type != TokenType.RightCurlyBracket)
            block.Children.Add(Walk());

        if (token.Type != TokenType.RightCurlyBracket)
            throw Error("No closing bracket found for block starting at line {0}", blockStartLine);

        node.Children.Add(block);

        Consume(TokenType.RightCurlyBracket);

        return node;
    }

    private bool CheckPrimitive()
    {
        return PeekType(0, TokenType.Number, TokenType.Identifier);
    }

    private ASTNode ParsePrimitive()
    {
        var prim = token.Type switch
        {
            TokenType.Identifier => new ASTNode(NodeType.Identifier, token.Content),
            TokenType.Number => new ASTNode(NodeType.Value, token.Content),
        };

        Advance();

        return prim;
    }
}
