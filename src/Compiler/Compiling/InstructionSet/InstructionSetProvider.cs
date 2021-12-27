using CompilerTest.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    internal class InstructionSetProvider
    {
        public static IInstructionSet LoadInstructionSet(string content)
        {
            IInstructionSet instructionSet = null;

            switch (content.SplitLines().First().ToLower())
            {
                case "simple":
                    instructionSet = new SimpleInstructionSet(content);
                    break;
                default:
                    break;
            }

            return instructionSet;
        }
    }
}
