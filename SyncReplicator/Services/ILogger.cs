namespace FolderSync.Services
{
    /// <summary>
    /// Interface for logging operations
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to both console and file
        /// </summary>
        /// <param name="message">Message to log</param>
        void Log(string message);
    }
}
