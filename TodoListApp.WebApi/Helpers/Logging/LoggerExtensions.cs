public static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Trace,
        Message = "Received request to {actionName}")]
    public static partial void LogTrace(ILogger logger, string actionName);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "An error has been accured.")]
    public static partial void LogError(ILogger logger);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Warning,
        Message = "{details}")]
    public static partial void LogWarning(ILogger logger, string details);
}
