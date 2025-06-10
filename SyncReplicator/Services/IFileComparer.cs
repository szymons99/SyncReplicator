namespace SyncReplicator.Services
{
    /// <summary>
    /// Interface for comparing files
    /// </summary>
    public interface IFileComparer
    {
        /// <summary>
        /// Compares two files to determine if they are identical
        /// </summary>
        /// <param name="file1">Path to first file</param>
        /// <param name="file2">Path to second file</param>
        /// <returns>True if files are identical, false otherwise</returns>
        bool AreFilesEqual(string file1, string file2);
    }
}