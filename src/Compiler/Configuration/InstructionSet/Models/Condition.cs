using CompilerTest.Compiling.Transformation.Enums;
using System.Text.Json.Serialization;

namespace CompilerTest.Configuration.InstructionSet.Models;

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
