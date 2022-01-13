using CompilerTest.Compiling.Transformation.Enums;
using System.Text.Json.Serialization;

namespace CompilerTest.Compiling.InstructionSet.Models
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
