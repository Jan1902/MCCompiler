﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Environment
{
    class CompilationEnvironment
    {
        public ImmutableList<Variable> SystemVariables { get; set; }

        public List<Variable> CustomVariables { get; set; }
        public Dictionary<string, int> Tags { get; set; }

        public CompilationEnvironment()
        {
            SystemVariables = new List<Variable>
            {
                new Variable("test", 0, true)
            }.ToImmutableList();

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
            return CustomVariables.FirstOrDefault(v => v.Name == name);
        }

        public Variable CreateVariable(string name)
        {
            var variable =
                new Variable(name,
                    false,
                    CustomVariables.Any()
                        ? CustomVariables.Max(v => v.Address) + 1
                        : 1);

            CustomVariables.Add(variable);
            return variable;
        }
    }
}