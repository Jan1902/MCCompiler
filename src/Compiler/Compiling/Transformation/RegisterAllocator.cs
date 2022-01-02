using CompilerTest.Compiling.Environment;
using CompilerTest.Compiling.Transformation.Models.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation
{
    internal class RegisterAllocator
    {
        private readonly List<Operations> ignorableOperations = new List<Operations>
        {
            Operations.Branch,
            Operations.NoOperation,
            Operations.Label,
            Operations.Halt,
        };

        public IntermediateInstruction[] AllocateRegisters_Old(IntermediateInstruction[] instructions)
        {
            var blocks = new List<Block>();

            // Detect blocks
            var currentBlock = new Block();
            for (int i = 0; i < instructions.Length; i++)
            {
                if (i != 0 && (instructions[i - 1].Operation == Operations.Branch
                    || instructions[i].Operation == Operations.Label))
                {
                    blocks.Add(currentBlock);
                    currentBlock = new Block();
                }

                currentBlock.Instructions.Add(instructions[i]);
            }
            blocks.Add(currentBlock);

            // Assign input and output variables
            var liveVariables = new Dictionary<Block, (List<Variable>, List<Variable>)>();

            foreach (var block in blocks)
            {
                var varsIn = new List<Variable>();
                var varsOut = new List<Variable>();

                foreach (var variable in GetUsedVariables(block))
                {
                    for (int i = 0; i < blocks.IndexOf(block); i++)
                    {
                        if (GetUsedVariables(blocks[i]).Contains(variable))
                            varsIn.Add(variable);
                    }

                    for (int i = blocks.IndexOf(block) + 1; i < blocks.Count; i++)
                    {
                        if (GetUsedVariables(blocks[i]).Contains(variable))
                            varsOut.Add(variable);
                    }
                }

                liveVariables.Add(block, (varsIn.Distinct().ToList(), varsOut.Distinct().ToList()));
            }

            // Generate Life Variable Graph
            var graph = new List<VariableNode>();
            foreach (var block in blocks)
            {
                foreach (var variable in liveVariables[block].Item1.Concat(liveVariables[block].Item2).Distinct())
                {
                    var node = graph.FirstOrDefault(n => n.Variable == variable);

                    if (node == null)
                    {
                        node = new VariableNode(variable);
                        graph.Add(node);
                    }

                    //node.Connected.AddRange(liveVariables[block].Item1.Where(v => v != variable && !node.Connected.Any(c => c.Variable.Equals(v))));
                }
            }

            // Peform Graph Coloring
            var k = 7;

            // Assign Registers

            return blocks.SelectMany(b => b.Instructions).ToArray();
        }
        public IntermediateInstruction[] AllocateRegisters(List<IntermediateInstruction> instructions)
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

            // Perform Graph Coloring
            int k = 7;
            var stack = new List<VariableNode>();
            var spilled = new List<Variable>();

            while (graph.Any(n => n.Color == 0))
            {
                foreach (var node in graph)
                {
                    for (int i = 1; i <= k; i++)
                    {
                        if(!node.Connected.Any(n => n.Color == i))
                            node.Color = i;
                    }

                    if(node.Color == 0)
                    {
                        var spill = graph.OrderBy(n => n.Connected.Count).First();
                        graph.Remove(spill);

                        foreach (var otherNode in graph)
                        {
                            if (otherNode.Connected.Contains(spill))
                                otherNode.Connected.Remove(spill);
                        }

                        spilled.Add(spill.Variable);

                        break;
                    }
                }
            }

            // Assign Addresses
            foreach (var node in graph)
                node.Variable.RegisterAddress = node.Color;

            for (int i = 0; i < spilled.Count; i++)
                spilled[i].RAMAddress = i;

            #region oldcoloring
            //while (graph.Count > 0)
            //{
            //    var node = graph.FirstOrDefault(n => n.Connected.Count < k);

            //    if (node == null)
            //    {
            //        node = graph.OrderBy(n => n.Connected.Count).First();
            //        graph.Remove(node);

            //        foreach (var otherNode in graph)
            //        {
            //            if (otherNode.Connected.Contains(node))
            //                otherNode.Connected.Remove(node);
            //        }

            //        spilled.Add(node.Variable);
            //        continue;
            //    }

            //    graph.Remove(node);
            //    stack.Add(node);
            //}

            //for (int i = stack.Count - 1; i >= 0; i--)
            //{
            //    stack[i].Variable.RegisterAddress = i;
            //}

            //for (int i = 0; i < spilled.Count; i++)
            //{
            //    spilled[i].RAMAddress = i;
            //}
            #endregion

            // Add MST and MLD
            for (int i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];

                var vars = instruction.Parameters.Skip(1).Where(p => p is Variable).Select(p => (Variable)p);
                foreach (var variable in vars.Where(v => spilled.Contains(v)))
                {
                    instructions.Insert(i, new IntermediateInstruction(Operations.MemoryLoad, variable.RegisterAddress, variable.RAMAddress));
                    i++;
                }

                if (instruction.Parameters.Any() && instruction.Parameters[0] is Variable v && spilled.Contains(v))
                {
                    if (instructions.Count > i + 1)
                        instructions.Insert(i + 1, new IntermediateInstruction(Operations.MemoryStore, v.RegisterAddress, v.RAMAddress));
                    else
                        instructions.Add(new IntermediateInstruction(Operations.MemoryStore, v.RegisterAddress, v.RAMAddress));

                    i++;
                }
            }

            // Replace variables with addresses
            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Parameters.Length; i++)
                {
                    if(instruction.Parameters[i] is Variable v)
                    {
                        instruction.Parameters[i] = v.RegisterAddress;
                    }
                }
            }

            return instructions.ToArray();
        }

        private List<Variable> GetUsedVariables(Block block)
        {
            var used = new List<Variable>();

            for (int i = 0; i < block.Instructions.Count; i++)
            {
                foreach (var variable in block.Instructions[i].Parameters.Skip(1).Where(p => p is Variable).Select(p => (Variable)p))
                {
                    for (int z = 0; z < i; z++)
                    {
                        if (!ignorableOperations.Contains(block.Instructions[z].Operation) && block.Instructions[z].Parameters[0].Equals(variable))
                            continue;
                    }
                    used.Add(variable);
                }
            }

            return used;
        }
    }
}
