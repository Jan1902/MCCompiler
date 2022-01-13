using CompilerTest.Compiling.Environment.Models;
using CompilerTest.Compiling.Transformation.Enums;
using CompilerTest.Compiling.Transformation.Models;
using CompilerTest.Compiling.Transformation.Models.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilerTest.Compiling.CodeGeneration.RegisterAllocation.Implementations
{
    internal class BasicRegisterAllocator : IRegisterAllocator
    {
        private readonly List<Operations> ignorableOperations = new List<Operations>
        {
            Operations.BGE,
            Operations.BNZ,
            Operations.BRZ,
            Operations.NOP,
            Operations.LBL,
            Operations.HLT,
        };

        public List<IntermediateInstruction> AllocateRegisters(List<IntermediateInstruction> instructions)
        {
            var nextSpillAddress = 0;
            var spilled = new List<VariableNode>();

            while (true)
            {
                var currentlyLive = new List<Variable>();
                var liveVariables = new Dictionary<IntermediateInstruction, List<Variable>>();

                // Detect live variables at every instruction
                instructions.Reverse();
                foreach (var instruction in instructions)
                {
                    if (!ignorableOperations.Contains(instruction.Operation))
                    {
                        if (instruction.Parameters[0] is Variable writeTo && currentlyLive.Contains(writeTo))
                            currentlyLive.Remove(writeTo);

                        foreach (var variable in instruction.Parameters.Skip(1).Where(p => p is Variable).Select(p => (Variable)p))
                        {
                            if (!currentlyLive.Contains(variable))
                                currentlyLive.Add(variable);
                        }
                    }

                    liveVariables[instruction] = new List<Variable>();

                    foreach (var variable in currentlyLive)
                        liveVariables[instruction].Add(variable);
                }
                instructions.Reverse();

                // Generate live variable graph
                var graph = new List<VariableNode>();
                var stack = new List<VariableNode>();
                var k = 7;

                foreach (var instruction in instructions)
                {
                    foreach (var variable in liveVariables[instruction])
                    {
                        var node = graph.FirstOrDefault(v => v.Variable.Equals(variable));

                        if (node == null)
                        {
                            node = new VariableNode(variable);
                            graph.Add(node);
                        }

                        foreach (var otherVariable in liveVariables[instruction].Where(v => !v.Equals(variable)))
                        {
                            var otherNode = graph.FirstOrDefault(n => n.Variable.Equals(otherVariable));

                            if (otherNode == null)
                            {
                                otherNode = new VariableNode(otherVariable);
                                graph.Add(otherNode);
                            }

                            if (!node.Connected.Contains(otherNode))
                                node.Connected.Add(otherNode);
                        }
                    }
                }

                foreach (var node in graph)
                    node.OriginallyConnected = new List<VariableNode>(node.Connected);

                // Perfom spilling if necessary
                var didSpill = false;
                while(graph.Any())
                {
                    var currentNode = graph.FirstOrDefault(n => n.Connected.Count < k);

                    if (currentNode == null)
                    {
                        for (int i = 0; i < instructions.Count; i++)
                        {
                            if (instructions[i].Parameters.Skip(1).Where(p => p is Variable).Select(p => (Variable)p).Contains(currentNode.Variable))
                            {
                                instructions.Insert(i, new IntermediateInstruction(Operations.LOD, currentNode.Variable, nextSpillAddress));
                                i++;

                                if (instructions.Count > i + 1)
                                    instructions.Insert(i + 1, new IntermediateInstruction(Operations.STR, currentNode.Variable, nextSpillAddress));
                                else
                                    instructions.Add(new IntermediateInstruction(Operations.STR, currentNode.Variable, nextSpillAddress));

                                nextSpillAddress++;
                            }
                        }

                        spilled.Add(currentNode);

                        break;
                    }

                    stack.Add(currentNode);
                    graph.Remove(currentNode);

                    foreach (var neighbour in currentNode.Connected)
                        neighbour.Connected.Remove(currentNode);
                }

                // Assign Registers with Greedy Coloring
                if (!didSpill)
                {
                    foreach (var node in stack)
                    {
                        var foundColor = false;
                        for (int i = 1; i <= k; i++)
                        {
                            if (!node.OriginallyConnected.Any(n => n.Variable.RegisterAddress == i))
                            {
                                node.Variable.RegisterAddress = i;
                                foundColor = true;
                                break;
                            }
                        }

                        if (!foundColor)
                            throw new Exception("Greedy Coloring failed");
                    }
                    break;
                }
            }

            // Replace variables with addresses
            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Parameters.Length; i++)
                {
                    if (instruction.Parameters[i] is Variable v)
                    {
                        instruction.Parameters[i] = v.RegisterAddress;
                    }
                }
            }

            return instructions;
        }
    }
}
