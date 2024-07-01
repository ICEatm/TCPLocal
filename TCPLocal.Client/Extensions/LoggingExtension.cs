using Microsoft.Extensions.Logging;
using System;

namespace TCPLocal.Client.Extensions
{
    /// <summary>
    /// Extension methods for logging operations.
    /// </summary>
    public static class LoggingExtension
    {
        /// <summary>
        /// Logs an error message without including the stack trace of the exception.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
        public static void LogErrorWithoutStackTrace(this ILogger logger, string message, Exception exception)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (!logger.IsEnabled(LogLevel.Error))
            {
                return;
            }

            logger.Log(LogLevel.Error, new EventId(0), message + " " + exception.Message, null, (s, ex) => s);
        }
    }
}
