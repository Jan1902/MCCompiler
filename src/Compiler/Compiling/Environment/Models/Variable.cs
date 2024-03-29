﻿namespace CompilerTest.Compiling.Environment.Models
{
    class Variable
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool ReadOnly { get; set; }
        public int RegisterAddress { get; set; }
        public int RAMAddress { get; set; }

        public Variable(string name, bool readOnly, int address)
        {
            Name = name;
            ReadOnly = readOnly;
            RegisterAddress = address;
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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(Variable))
                return false;

            return ((Variable)obj).Name == Name;
        }
    }
}
