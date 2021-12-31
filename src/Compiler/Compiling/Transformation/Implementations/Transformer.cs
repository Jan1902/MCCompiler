using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation.Implementations
{
    internal class Transformer : ITransformer
    {
        private readonly CompilationEnvironment _environment;

        private readonly List<RawInstruction> result;

        public Transformer()
        {
            result = new List<RawInstruction>();
            _environment = new CompilationEnvironment();
        }

        public List<RawInstruction> Transform(Node node)
        {
            var result = new List<RawInstruction>();

            switch (node.Type)
            {
                case NodeType.Assignment:
                    {
                        var variable = _environment.GetOrCreateVariable(node.Children[0].Value);
                        if (node.Children[1].Type == NodeType.Value)
                            result.Add(new RawInstruction(Operations.LoadImmediate, variable.Address, int.Parse(node.Children[1].Value)));

                        else if (node.Children[1].Type == NodeType.Increment)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);
                            result.Add(new RawInstruction(Operations.Increment, variable.Address, sourceVariable.Address));
                        }

                        else if (node.Children[1].Type == NodeType.Shift)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                            result.Add(new RawInstruction(node.Children[1].Children[1].Value == ">" ? Operations.ShiftRight : Operations.ShiftLeft, variable.Address, sourceVariable.Address));
                        }

                        else if ((node.Children[1].Type == NodeType.Addition
                            || node.Children[1].Type == NodeType.Subtraction)
                            && node.Children[1].Children.Count == 2)
                        {
                            var aVariable = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                            var bVariable = _environment.GetVariableByName(node.Children[1].Children[1].Value);

                            result.Add(new RawInstruction(node.Children[1].Type == NodeType.Addition ? Operations.Add : Operations.Subtract, variable.Address, aVariable.Address, bVariable.Address));
                        }

                        else if (node.Children[1].Type == NodeType.Input)
                        {
                            result.Add(new RawInstruction(Operations.PortLoad, variable.Address, int.Parse(node.Children[1].Children[0].Value)));
                        }
                        break;
                    }

                case NodeType.Output:
                    {
                        var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);
                        result.Add(new RawInstruction(Operations.PortStore, int.Parse(node.Children[0].Value), sourceVariable.Address));
                    }
                    break;

                case NodeType.Program:
                    foreach (var child in node.Children)
                        result.AddRange(Transform(child));
                    break;

                case NodeType.ConditionalStatement:
                    var conditionalBlockStart = result.Count;

                    foreach (var child in node.Children[1].Children)
                        result.AddRange(Transform(child));

                    TranslateCondition(conditionalBlockStart);

                    break;

                case NodeType.WhileLoop:
                    var loopBlockStart = result.Count;

                    foreach (var child in node.Children[1].Children)
                        result.AddRange(Transform(child));

                    TranslateCondition(loopBlockStart);

                    result.Add(new RawInstruction(Operations.Branch, Conditions.NoCondition, loopBlockStart));
                    break;

                case NodeType.Halt:
                    result.Add(new RawInstruction(Operations.Halt));
                    break;

                default:
                    throw new Exception();
            }

            void TranslateCondition(int blockStart)
            {
                var aVariable = _environment.GetVariableByName(node.Children[0].Children[0].Value);
                var bVariable = _environment.GetVariableByName(node.Children[0].Children[2].Value);

                if (node.Children[0].Children[1].Value == ">="
                    || node.Children[0].Children[1].Value == "<"
                    || node.Children[0].Children[1].Value == "==")
                    result.Insert(blockStart, new RawInstruction(Operations.Subtract,
                        0, aVariable.Address, bVariable.Address));
                else
                    result.Insert(blockStart, new RawInstruction(Operations.Subtract,
                        0, bVariable.Address, aVariable.Address));

                if (node.Children[0].Children[1].Value == ">"
                    || node.Children[0].Children[1].Value == "<")
                    result.Insert(blockStart + 1, new RawInstruction(Operations.Branch, Conditions.CarryOut, result.Count + 1));
                else if (node.Children[0].Children[1].Value == ">="
                    || node.Children[0].Children[1].Value == "<=")
                    result.Insert(blockStart + 1, new RawInstruction(Operations.Branch, Conditions.NoCarryOut, result.Count + 1));
                else
                    result.Insert(blockStart + 1, new RawInstruction(Operations.Branch, Conditions.NotZero, result.Count + 1));
            }

            if (node.Type == NodeType.Program)
                result.Add(new RawInstruction(Operations.Halt));

            return result;
        }
    }
}
