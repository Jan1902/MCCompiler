using System;

namespace CompilerTest.Utility;

public static class Extensions
{
    public static string[] SplitLines(this string text)
    {
        return text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }
}
