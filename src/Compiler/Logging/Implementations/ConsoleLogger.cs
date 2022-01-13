using System;

namespace CompilerTest.Logging.Implementations
{
    internal class ConsoleLogger : ILoggerImplementation
    {
        public void LogInfo(string text, params object[] args)
        {
            Console.ResetColor();
            Console.Write("[*] ");
            Console.WriteLine(string.Format(text.ToString(), args));
        }

        public void LogWarning(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[?] ");
            Console.WriteLine(string.Format(text.ToString(), args));
        }

        public void LogSuccess(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[S] ");
            Console.WriteLine(string.Format(text.ToString(), args));
            Console.ResetColor();
        }

        public void LogError(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[!] ");
            Console.WriteLine(string.Format(text.ToString(), args));
            Console.ResetColor();
        }
    }
}
