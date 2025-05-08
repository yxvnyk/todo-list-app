using Microsoft.Extensions.Logging;

namespace TodoListApp.WebApi.Models.Logging;

public static partial class LoggerExtensions
{
    /// <summary>
    /// Logs a trace-level message indicating a request was received for a specified action.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="actionName">The name of the action being requested.</param>
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Trace,
        Message = "Received request to {actionName}")]
    public static partial void LogTrace(ILogger logger, string actionName);

    /// <summary>
    /// Logs an error-level message with details provided about the error.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="details">Details about the error.</param>
    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "{details}.")]
    public static partial void LogError(ILogger logger, string details);

    /// <summary>
    /// Logs a warning-level message with specific details about the potential issue.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="details">Details about the warning.</param>
    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Warning,
        Message = "{details}")]
    public static partial void LogWarning(ILogger logger, string details);
}
