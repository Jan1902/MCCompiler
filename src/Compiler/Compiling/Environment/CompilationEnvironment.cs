using CompilerTest.Compiling.Environment.Models;
using System.Collections.Generic;
using System.Linq;

namespace CompilerTest.Compiling.Environment;

class CompilationEnvironment
{
    public List<Variable> ConstantVariables { get; set; }

    public List<Variable> CustomVariables { get; set; }
    public Dictionary<string, int> Tags { get; set; }

    public CompilationEnvironment()
    {
        ConstantVariables = new List<Variable>
        {
            new Variable("DisplayX", 0, true),
            new Variable("DisplayY", 1, true),
            new Variable("Text", 2, true)
        };

        CustomVariables = new List<Variable>();
        Tags = new Dictionary<string, int>();
    }

    public Variable GetOrCreateVariable(string name)
    {
        var variable = GetVariableByName(name);

        if (variable == null)
            variable = CreateVariable(name);

        return variable;
    }

    public Variable GetVariableByName(string name)
    {
        return CustomVariables.Concat(ConstantVariables).FirstOrDefault(v => v.Name == name);
    }

    public Variable CreateConstant(string name, int value)
    {
        var constant = new Variable(name, value, true);

        ConstantVariables.Add(constant);

        return constant;
    }

    public Variable CreateVariable(string name)
    {
        var variable =
            new Variable(name,
                false,
                0);

        CustomVariables.Add(variable);
        return variable;
    }
}
