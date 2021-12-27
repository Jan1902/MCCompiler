using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Environment
{
    class Variable
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool ReadOnly { get; set; }
        public int Address { get; set; }

        public Variable(string name, bool readOnly, int address)
        {
            Name = name;
            ReadOnly = readOnly;
            Address = address;
        }

        public Variable(string name, int value, bool readOnly)
        {
            Name = name;
            Value = value;
            ReadOnly = readOnly;
        }

        public Variable()
        {

        }
    }
}
