﻿using CompilerTest.Compiling.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    internal class InstructionSet : IInstructionSet
    {
        public ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();
        public ICollection<Condition> Conditions { get; set; } = new List<Condition>();

        public Instruction GetInstruction(Operations operation) => Instructions.FirstOrDefault(i => i.Operation == operation);

        public Condition GetCondition(Conditions condition) => Conditions.FirstOrDefault(i => i.ConditionType == condition);
    }
}