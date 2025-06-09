using System;
using System.IO;
using System.Security.Cryptography;

namespace FolderSync.Services
{
    /// <summary>
    /// File comparer using MD5 hash algorithm
    /// </summary>
    public class MD5FileComparer : IFileComparer
    {
        /// <summary>
        /// Compares two files using MD5 hash
        /// </summary>
        /// <param name="file1">Path to first file</param>
        /// <param name="file2">Path to second file</param>
        /// <returns>True if files have the same MD5 hash</returns>
        public bool AreFilesEqual(string file1, string file2)
        {
            // If second file doesn't exist, files are not equal
            if (!File.Exists(file2))
                return false;

            // Compare MD5 hashes
            string hash1 = CalculateMD5(file1);
            string hash2 = CalculateMD5(file2);

            return hash1.Equals(hash2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Calculates MD5 hash of a file
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>MD5 hash as lowercase string</returns>
        private string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
