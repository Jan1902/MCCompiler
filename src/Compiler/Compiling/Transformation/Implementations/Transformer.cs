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

        private readonly List<IntermediateInstruction> result;

        private int labelIndex;

        public Transformer()
        {
            result = new List<IntermediateInstruction>();
            _environment = new CompilationEnvironment();
        }

        public List<IntermediateInstruction> Transform(Node node)
        {
            var result = new List<IntermediateInstruction>();

            switch (node.Type)
            {
                case NodeType.Assignment:
                    {
                        var variable = _environment.GetOrCreateVariable(node.Children[0].Value);
                        if (node.Children[1].Type == NodeType.Value)
                            result.Add(new IntermediateInstruction(Operations.LoadImmediate, variable, int.Parse(node.Children[1].Value)));

                        else if (node.Children[1].Type == NodeType.Increment)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);
                            result.Add(new IntermediateInstruction(Operations.Increment, variable, sourceVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Shift)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                            result.Add(new IntermediateInstruction(node.Children[1].Children[1].Value == ">" ? Operations.ShiftRight : Operations.ShiftLeft, variable, sourceVariable));
                        }

                        else if ((node.Children[1].Type == NodeType.Addition
                            || node.Children[1].Type == NodeType.Subtraction)
                            && node.Children[1].Children.Count == 2)
                        {
                            var aVariable = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                            var bVariable = _environment.GetVariableByName(node.Children[1].Children[1].Value);

                            result.Add(new IntermediateInstruction(node.Children[1].Type == NodeType.Addition ? Operations.Add : Operations.Subtract, variable, aVariable, bVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Input)
                        {
                            result.Add(new IntermediateInstruction(Operations.PortLoad, variable, int.Parse(node.Children[1].Children[0].Value)));
                        }
                        break;
                    }

                case NodeType.Output:
                    {
                        var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);
                        result.Add(new IntermediateInstruction(Operations.PortStore, int.Parse(node.Children[0].Value), sourceVariable));
                    }
                    break;

                case NodeType.Program:
                    foreach (var child in node.Children)
                        result.AddRange(Transform(child));
                    break;

                case NodeType.ConditionalStatement:
                    var conditionalBlockStart = result.Count;
                    var currentLabelIndexConditional = labelIndex;

                    TranslateCondition(conditionalBlockStart, "ce" + currentLabelIndexConditional);

                    foreach (var child in node.Children[1].Children)
                        result.AddRange(Transform(child));

                    result.Add(new IntermediateInstruction(Operations.Label, "ce" + currentLabelIndexConditional));

                    labelIndex++;
                    break;

                case NodeType.WhileLoop:
                    var loopBlockStart = result.Count;
                    var currentLabelIndexLoop = labelIndex;

                    result.Add(new IntermediateInstruction(Operations.Label, "ls" + currentLabelIndexLoop));
                    TranslateCondition(loopBlockStart, "le" + currentLabelIndexLoop);

                    foreach (var child in node.Children[1].Children)
                        result.AddRange(Transform(child));

                    result.Add(new IntermediateInstruction(Operations.Branch, Conditions.NoCondition, "ls" + currentLabelIndexLoop));
                    result.Add(new IntermediateInstruction(Operations.Label, "le" + currentLabelIndexLoop));

                    labelIndex++;
                    break;

                case NodeType.Halt:
                    result.Add(new IntermediateInstruction(Operations.Halt));
                    break;

                default:
                    throw new Exception();
            }

            void TranslateCondition(int blockStart, string elseLabel)
            {
                var aVariable = _environment.GetVariableByName(node.Children[0].Children[0].Value);
                var bVariable = _environment.GetVariableByName(node.Children[0].Children[2].Value);

                if (node.Children[0].Children[1].Value == ">="
                    || node.Children[0].Children[1].Value == "<"
                    || node.Children[0].Children[1].Value == "==")
                    result.Add(new IntermediateInstruction(Operations.Subtract,
                        0, aVariable, bVariable));
                else
                    result.Add(new IntermediateInstruction(Operations.Subtract,
                        0, bVariable, aVariable));

                if (node.Children[0].Children[1].Value == ">"
                    || node.Children[0].Children[1].Value == "<")
                    result.Add(new IntermediateInstruction(Operations.Branch, Conditions.CarryOut, elseLabel));
                else if (node.Children[0].Children[1].Value == ">="
                    || node.Children[0].Children[1].Value == "<=")
                    result.Add(new IntermediateInstruction(Operations.Branch, Conditions.NoCarryOut, elseLabel));
                else
                    result.Add(new IntermediateInstruction(Operations.Branch, Conditions.NotZero, elseLabel));
            }

            if (node.Type == NodeType.Program)
                result.Add(new IntermediateInstruction(Operations.Halt));

            return result;
        }
    }
}
