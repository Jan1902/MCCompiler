using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.Parsing.Models;
using CompilerTest.Compiling.Transformation.Enums;
using CompilerTest.Compiling.Transformation.Models;
using System;
using System.Collections.Generic;

namespace CompilerTest.Compiling.Transformation.Implementations
{
    internal class Transformer : ITransformer
    {
        private readonly CompilationEnvironment _environment;

        private int labelIndex;

        private Dictionary<string, Operations> arithmetics = new Dictionary<string, Operations>()
        {
            { "+", Operations.ADD },
            { "-", Operations.SUB },
            { "*", Operations.MLT },
            { "/", Operations.DIV },
            { "%", Operations.MOD },
        };

        private Dictionary<string, (Operations, Operations)> branchTypes = new Dictionary<string, (Operations, Operations)>()
        {
            { ">", (Operations.BRG, Operations.BLE) },
            { ">=", (Operations.BGE, Operations.BRL) },
            { "<", (Operations.BRL, Operations.BGE) },
            { "<=", (Operations.BLE, Operations.BRG) },
            { "==", (Operations.BRE, Operations.BNE) },
            { "!=", (Operations.BNE, Operations.BRE) }
        };

        public Transformer(CompilationEnvironment environment)
        {
            _environment = environment;
        }

        public List<IntermediateInstruction> Transform(ASTNode node)
        {
            var result = new List<IntermediateInstruction>();

            switch (node.Type)
            {
                case NodeType.ConstantAssignment:
                    _environment.CreateConstant(node.Children[0].Value, int.Parse(node.Children[1].Value));
                    break;

                case NodeType.Assignment:
                    {
                        var variable = _environment.GetOrCreateVariable(node.Children[0].Value);
                        if (node.Children[1].Type == NodeType.Value)
                            result.Add(new IntermediateInstruction(Operations.IMM, variable, int.Parse(node.Children[1].Value)));

                        else if (node.Children[1].Type == NodeType.Identifier)
                        {
                            var aVariable = _environment.GetVariableByName(node.Children[0].Value);
                            var bVariable = _environment.GetVariableByName(node.Children[1].Value);

                            result.Add(new IntermediateInstruction(Operations.MOV, aVariable, bVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Increment)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);
                            result.Add(new IntermediateInstruction(Operations.INC, variable, sourceVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Decrement)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);
                            result.Add(new IntermediateInstruction(Operations.DEC, variable, sourceVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Shift)
                        {
                            var sourceVariable = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                            result.Add(new IntermediateInstruction(node.Children[1].Children[1].Value == ">" ? Operations.RSH : Operations.LSH, variable, sourceVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Arithmetic
                            && node.Children[1].Children.Count == 3)
                        {
                            var aVariable = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                            var bVariable = _environment.GetVariableByName(node.Children[1].Children[2].Value);

                            result.Add(new IntermediateInstruction(arithmetics[node.Children[1].Children[1].Value], variable, aVariable, bVariable));
                        }

                        else if (node.Children[1].Type == NodeType.Input)
                        {
                            object port = null;
                            if (int.TryParse(node.Children[1].Children[0].Value, out _))
                                port = int.Parse(node.Children[1].Children[0].Value);
                            else
                            {
                                var portVar = _environment.GetVariableByName(node.Children[1].Children[0].Value);
                                port = portVar.ReadOnly ? portVar.Value : portVar;
                            }

                            result.Add(new IntermediateInstruction(Operations.IN, variable, port));
                        }
                        break;
                    }

                case NodeType.Output:
                    {
                        var sourceVariable = _environment.GetVariableByName(node.Children[1].Value);

                        object port = null;
                        if (int.TryParse(node.Children[0].Value, out _))
                            port = int.Parse(node.Children[0].Value);
                        else
                        {
                            var portVar = _environment.GetVariableByName(node.Children[0].Value);
                            port = portVar.ReadOnly ? portVar.Value : portVar;
                        }

                        result.Add(new IntermediateInstruction(Operations.OUT, port, sourceVariable));
                    }
                    break;

                case NodeType.Root:
                    foreach (var child in node.Children)
                        result.AddRange(Transform(child));
                    break;

                case NodeType.ConditionalStatement:
                    var conditionalBlockStart = result.Count;
                    var currentLabelIndexConditional = labelIndex;

                    TranslateCondition(conditionalBlockStart, "ce" + currentLabelIndexConditional);

                    foreach (var child in node.Children[1].Children)
                        result.AddRange(Transform(child));

                    result.Add(new IntermediateInstruction(Operations.LBL, "ce" + currentLabelIndexConditional));

                    labelIndex++;
                    break;

                case NodeType.WhileLoop:
                    var loopBlockStart = result.Count;
                    var currentLabelIndexLoop = labelIndex;

                    result.Add(new IntermediateInstruction(Operations.LBL, "ls" + currentLabelIndexLoop));
                    TranslateCondition(loopBlockStart, "le" + currentLabelIndexLoop);

                    foreach (var child in node.Children[1].Children)
                        result.AddRange(Transform(child));

                    result.Add(new IntermediateInstruction(Operations.JMP, "ls" + currentLabelIndexLoop));
                    result.Add(new IntermediateInstruction(Operations.LBL, "le" + currentLabelIndexLoop));

                    labelIndex++;
                    break;

                case NodeType.Halt:
                    result.Add(new IntermediateInstruction(Operations.HLT));
                    break;

                default:
                    throw new Exception();
            }

            if (node.Type == NodeType.Root)
                result.Add(new IntermediateInstruction(Operations.HLT));

            void TranslateCondition(int blockStart, string elseLabel)
            {
                var aVariable = _environment.GetVariableByName(node.Children[0].Children[0].Value);
                var bVariable = _environment.GetVariableByName(node.Children[0].Children[2].Value);

                result.Add(new IntermediateInstruction(branchTypes[node.Children[0].Children[1].Value].Item2, elseLabel, aVariable, bVariable));
            }

            return result;
        }
    }
}
