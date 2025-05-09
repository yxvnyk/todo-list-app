using Microsoft.Extensions.Logging;

namespace TodoListApp.WebApi.Models.Logging
{
    /// <summary>
    /// Provides extension methods for logging with various log levels.
    /// </summary>
    public static partial class LoggerExtensions
    {
        /// <summary>
        /// Defines a log action for logging errors.
        /// </summary>
        private static readonly Action<ILogger, string, Exception?> Error =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1003, "Error logging"),
            "An error has occured: {Class}!");

        /// <summary>
        /// Defines a log action for logging warnings.
        /// </summary>
        private static readonly Action<ILogger, string, Exception?> Warning =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1003, "Warning logging"),
            "{Class}!");

        /// <summary>
        /// Defines a log action for logging trace-level messages.
        /// </summary>
        private static readonly Action<ILogger, string, Exception?> Trace =
        LoggerMessage.Define<string>(
            LogLevel.Trace,
            new EventId(1003, "Trace logginf"),
            "Received request to {Class}");

        /// <summary>
        /// Logs an error message with the specified class name.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="class">The name of the class from which the log is generated.</param>
        public static void LogError(this ILogger logger, string @class)
        {
            Error(logger, @class, null);
        }

        /// <summary>
        /// Logs a warning message with the specified class name.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="class">The name of the class from which the log is generated.</param>
        public static void LogWarning(this ILogger logger, string @class)
        {
            Warning(logger, @class, null);
        }

        /// <summary>
        /// Logs a trace message with the specified class name.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="class">The name of the class from which the log is generated.</param>
        public static void LogTrace(this ILogger logger, string @class)
        {
            Trace(logger, @class, null);
        }
    }
}
