using CompilerTest.Compiling.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.InstructionSet
{
    public class Condition
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Conditions ConditionType { get; set; }
        public int OPCode { get; set; }

        public Condition(Conditions conditionType, int oPCode)
        {
            ConditionType = conditionType;
            OPCode = oPCode;
        }

        public Condition()
        {

        }
    }
}
