using CompilerTest.Compiling.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    public class Instruction
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Operations Operation { get; set; }
        public int OPCode { get; set; }
        public string Translation { get; set; }

        public Instruction(Operations operation, int oPCode, string translation)
        {
            Operation = operation;
            OPCode = oPCode;
            Translation = translation;
        }

        public Instruction()
        {

        }
    }
}
