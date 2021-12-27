using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Other
{
    public static class Extensions
    {
        public static string[] SplitLines(this string text)
        {
            return text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static string ReplaceMany(this string text, Dictionary<string, string> parts)
        {
            foreach (var part in parts)
                text = text.Replace(part.Key, part.Value);

            return text;
        }
    }
}
