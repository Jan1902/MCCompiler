using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Compiling
{
    internal interface ICompiler
    {
        string[] Compile(string code);
    }
}
