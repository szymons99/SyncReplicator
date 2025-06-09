namespace SyncReplicator.Models
{
    /// <summary>
    /// Configuration class containing all synchronization parameters
    /// </summary>
    public class SyncConfiguration
    {
        public string SourcePath { get; set; }
        public string ReplicaPath { get; set; }
        public int IntervalSeconds { get; set; }
        public string LogFilePath { get; set; }
    }
}
