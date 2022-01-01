using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchematicCreator.Utility
{
    internal class Utils
    {
        public static int CalculateIndex(int width, int length, int x, int y, int z) => (y * length + z) * width + x;
    }
}
