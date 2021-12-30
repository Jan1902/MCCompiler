﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation
{
    internal class RawInstruction
    {
        public string Operation { get; set; }
        public List<object> Parameters { get; set; }

        public RawInstruction(string operation, params object[] parameters)
        {
            Operation = operation;
            Parameters = parameters.ToList();
        }

        public RawInstruction()
        {

        }
    }
}