namespace CompilerTest.Logging.Implementations;

internal interface ILoggerImplementation
{
    void LogError(string text, params object[] args);
    void LogInfo(string text, params object[] args);
    void LogSuccess(string text, params object[] args);
    void LogWarning(string text, params object[] args);
}