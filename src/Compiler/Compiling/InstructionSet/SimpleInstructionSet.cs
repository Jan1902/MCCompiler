using CompilerTest.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    public class SimpleInstructionSet : IInstructionSet
    {
        public ICollection<Instruction> Instructions { get; set; }

        public SimpleInstructionSet(string content)
        {
            Load(content);
        }

        public SimpleInstructionSet()
        {

        }

        public void Load(string content)
        {
            Instructions = new List<Instruction>();
            var lines = content.SplitLines();

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(" ");
                var instruction = new Instruction();
                instruction.OPCode = int.Parse(parts[0]);
                instruction.Name = parts[1];

                if (parts.Length > 2)
                {
                    instruction.Translation = parts[2];
                }

                Instructions.Add(instruction);
            }
        }

        public Instruction GetInstructionByName(string name)
        {
            return Instructions.FirstOrDefault(i => i.Name == name);
        }
    }
}
