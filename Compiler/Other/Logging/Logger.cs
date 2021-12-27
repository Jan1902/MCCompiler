using CompilerTest.Other.Logging.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerTest.Other.Logging
{
    public class Logger : ILogger
    {
        private List<ILoggerImplementation> loggerImplementations;

        public Logger()
        {
            loggerImplementations = new List<ILoggerImplementation>();
            loggerImplementations.Add(new ConsoleLogger());
            loggerImplementations.Add(new FileLogger("log.txt"));
        }

        public void LogInfo(string text, params object[] args)
        {
            foreach (var implementation in loggerImplementations)
                implementation.LogInfo(text, args);
        }

        public void LogWarning(string text, params object[] args)
        {
            foreach (var implementation in loggerImplementations)
                implementation.LogWarning(text, args);
        }

        public void LogSuccess(string text, params object[] args)
        {
            foreach (var implementation in loggerImplementations)
                implementation.LogSuccess(text, args);
        }

        public void LogError(string text, params object[] args)
        {
            foreach (var implementation in loggerImplementations)
                implementation.LogError(text, args);
        }
    }
}
