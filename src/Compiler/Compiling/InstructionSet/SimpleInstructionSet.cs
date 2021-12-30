using CompilerTest.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    public class SimpleInstructionSet : IInstructionSet
    {
        public ICollection<Instruction> Instructions { get; set; }
        public ICollection<Condition> Conditions { get; set; }

        public SimpleInstructionSet(string content)
        {
            LoadInstructions(content);
            LoadConditions(content);
        }

        private void LoadInstructions(string content)
        {
            var instructionsContent = Regex.Match(content, @"(?<=-Instructions(\r\n|\n))([\s\S]*?)(?=(\r\n|\n)-)").Value;
            Instructions = new List<Instruction>();
            var lines = instructionsContent.SplitLines().ToList();

            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var instruction = new Instruction();
                instruction.OPCode = int.Parse(parts[0]);
                instruction.Name = parts[1];

                if (parts.Length > 2)
                    instruction.Translation = parts[2];

                Instructions.Add(instruction);
            }
        }

        private void LoadConditions(string content)
        {
            var conditionsContent = Regex.Match(content, @"(?<=-Conditions(\r\n|\n))([\s\S]*?)(?=(\r\n|\n)-)").Value;
            Conditions = new List<Condition>();
            var lines = conditionsContent.SplitLines().ToList();

            foreach (var line in lines)
            {
                var parts = line.Split(" ");
                var condition = new Condition();
                condition.OPCode = int.Parse(parts[0]);
                condition.Name = parts[1];

                Conditions.Add(condition);
            }
        }

        public Instruction GetInstructionByName(string name) => Instructions.FirstOrDefault(i => i.Name == name);

        public Condition GetConditionByName(string name) => Conditions.FirstOrDefault(i => i.Name == name);
    }
}
