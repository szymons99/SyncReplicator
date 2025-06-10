using System;
using SyncReplicator.Models;

namespace SyncReplicator.Utils
{
    /// <summary>
    /// Utility class for validating and parsing command line arguments
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// Validates command line arguments and returns configuration object
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>SyncConfiguration object or null if validation fails</returns>
        public static SyncConfiguration ValidateAndParse(string[] args)
        {
            // Check if all required arguments are provided
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: dotnet run <source_path> <replica_path> <interval_seconds> <log_file_path>");
                Console.WriteLine("Example: dotnet run C:\\Source C:\\Replica 30 C:\\Logs\\sync.log");
                return null;
            }

            // Parse interval to integer
            if (!int.TryParse(args[2], out int intervalSeconds) || intervalSeconds <= 0)
            {
                Console.WriteLine("Error: Interval must be a positive integer (seconds)");
                return null;
            }

            return new SyncConfiguration
            {
                SourcePath = args[0],
                ReplicaPath = args[1],
                IntervalSeconds = intervalSeconds,
                LogFilePath = args[3]
            };
        }
    }
}
