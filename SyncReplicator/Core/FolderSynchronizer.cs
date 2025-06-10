using System;
using System.IO;
using SyncReplicator.Models;
using SyncReplicator.Services;
using SyncReplicator.Models;

namespace SyncReplicator.Core
{
    /// <summary>
    /// Main class responsible for folder synchronization
    /// </summary>
    public class FolderSynchronizer
    {
        private readonly SyncConfiguration _config;
        private readonly ILogger _logger;
        private readonly IFileComparer _fileComparer;
        private readonly object _lockObject = new object();

        public FolderSynchronizer(SyncConfiguration config)
        {
            _config = config;
            _logger = new FileLogger(config.LogFilePath);
            _fileComparer = new MD5FileComparer();
        }

        /// <summary>
        /// Performs one-way synchronization from source to replica folder
        /// </summary>
        public void Synchronize()
        {
            lock (_lockObject)
            {
                try
                {
                    _logger.Log("=== Starting synchronization ===");

                    // Check if source folder exists
                    if (!Directory.Exists(_config.SourcePath))
                    {
                        _logger.Log($"ERROR: Source folder does not exist: {_config.SourcePath}");
                        return;
                    }

                    // Create replica folder if it doesn't exist
                    if (!Directory.Exists(_config.ReplicaPath))
                    {
                        Directory.CreateDirectory(_config.ReplicaPath);
                        _logger.Log($"Created replica folder: {_config.ReplicaPath}");
                    }

                    // Synchronize files and folders
                    SynchronizeDirectory(_config.SourcePath, _config.ReplicaPath);

                    // Remove extra items from replica that don't exist in source
                    RemoveExtraItems(_config.SourcePath, _config.ReplicaPath);

                    _logger.Log("=== Synchronization completed ===\n");
                }
                catch (Exception ex)
                {
                    _logger.Log($"ERROR during synchronization: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Recursively synchronizes directories
        /// </summary>
        /// <param name="sourceDir">Source directory path</param>
        /// <param name="replicaDir">Replica directory path</param>
        private void SynchronizeDirectory(string sourceDir, string replicaDir)
        {
            // Create directory in replica if it doesn't exist
            if (!Directory.Exists(replicaDir))
            {
                Directory.CreateDirectory(replicaDir);
                _logger.Log($"Created directory: {replicaDir}");
            }

            // Synchronize files
            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string replicaFile = Path.Combine(replicaDir, fileName);

                // Check if file needs to be copied
                if (!_fileComparer.AreFilesEqual(sourceFile, replicaFile))
                {
                    File.Copy(sourceFile, replicaFile, true);
                    _logger.Log($"Copied file: {sourceFile} -> {replicaFile}");
                }
            }

            // Recursively synchronize subdirectories
            foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(sourceSubDir);
                string replicaSubDir = Path.Combine(replicaDir, dirName);
                SynchronizeDirectory(sourceSubDir, replicaSubDir);
            }
        }

        /// <summary>
        /// Removes files and folders from replica that don't exist in source
        /// </summary>
        /// <param name="sourceDir">Source directory path</param>
        /// <param name="replicaDir">Replica directory path</param>
        private void RemoveExtraItems(string sourceDir, string replicaDir)
        {
            // Remove extra files
            foreach (string replicaFile in Directory.GetFiles(replicaDir))
            {
                string fileName = Path.GetFileName(replicaFile);
                string sourceFile = Path.Combine(sourceDir, fileName);

                if (!File.Exists(sourceFile))
                {
                    File.Delete(replicaFile);
                    _logger.Log($"Removed file: {replicaFile}");
                }
            }

            // Remove extra directories
            foreach (string replicaSubDir in Directory.GetDirectories(replicaDir))
            {
                string dirName = Path.GetFileName(replicaSubDir);
                string sourceSubDir = Path.Combine(sourceDir, dirName);

                if (!Directory.Exists(sourceSubDir))
                {
                    Directory.Delete(replicaSubDir, true);
                    _logger.Log($"Removed directory: {replicaSubDir}");
                }
                else
                {
                    // Recursively check subdirectories
                    RemoveExtraItems(sourceSubDir, replicaSubDir);
                }
            }
        }
    }
}
