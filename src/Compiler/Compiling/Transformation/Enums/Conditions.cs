using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling.Transformation
{
    public enum Conditions
    {
        NoCondition,
        CarryOut,
        NoCarryOut,
        Zero,
        NotZero,
        ShiftOverflow,
        ShiftUnderflow,
        NoShiftOverflow,
        NoShiftUnderflow
    }
}
