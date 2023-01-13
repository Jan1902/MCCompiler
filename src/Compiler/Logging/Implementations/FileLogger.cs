using System;
using System.IO;

namespace CompilerTest.Logging.Implementations;

internal class FileLogger : ILoggerImplementation
{
    private readonly string _filename;

    public FileLogger(string filename)
    {
        _filename = filename;
    }

    public void LogInfo(string text, params object[] args)
    {
        File.AppendAllText(_filename,
            DateTime.Now.ToShortDateString().Replace(".", "-")
            + " [*] "
            + string.Format(text.ToString(), args)
            + Environment.NewLine);
    }

    public void LogWarning(string text, params object[] args)
    {
        File.AppendAllText(_filename,
            DateTime.Now.ToShortDateString().Replace(".", "-")
            + " [?] "
            + string.Format(text.ToString(), args)
            + Environment.NewLine);
    }

    public void LogSuccess(string text, params object[] args)
    {
        File.AppendAllText(_filename,
            DateTime.Now.ToShortDateString().Replace(".", "-")
            + " [S] "
            + string.Format(text.ToString(), args)
            + Environment.NewLine);
    }

    public void LogError(string text, params object[] args)
    {
        File.AppendAllText(_filename,
            DateTime.Now.ToShortDateString().Replace(".", "-")
            + " [!] "
            + string.Format(text.ToString(), args)
            + Environment.NewLine);
    }
}
