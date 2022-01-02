using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation
{
    public enum Operations
    {
        NoOperation,
        Add,
        Subtract,
        ShiftRight,
        ShiftLeft,
        Increment,
        Decrement,
        PortStore,
        PortLoad,
        MemoryStore,
        MemoryLoad,
        LoadImmediate,
        AddImmediate,
        Branch,
        Label,
        Halt
    }
}
