using System;
using System.IO;

namespace SyncReplicator.Services
{
    /// <summary>
    /// Logger implementation that writes to both file and console
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;

            // Create log directory if it doesn't exist
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Logs message with timestamp to both console and file
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            lock (_lockObject)
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

                // Write to console
                Console.WriteLine(logEntry);

                // Write to file
                try
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }
    }
}
