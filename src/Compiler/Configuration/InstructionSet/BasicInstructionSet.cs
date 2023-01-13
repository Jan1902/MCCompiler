using CompilerTest.Compiling.Transformation.Enums;
using CompilerTest.Configuration.InstructionSet.Models;
using System.Collections.Generic;
using System.Linq;

namespace CompilerTest.Configuration.InstructionSet;

internal class BasicInstructionSet
{
    public ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();
    public ICollection<Condition> Conditions { get; set; } = new List<Condition>();

    public Instruction GetInstruction(Operations operation) => Instructions.FirstOrDefault(i => i.Operation == operation);

    public Condition GetCondition(Conditions condition) => Conditions.FirstOrDefault(i => i.ConditionType == condition);
}
