using System;
using System.Threading;
using SyncReplicator.Core;

namespace SyncReplicator.Utils
{
    /// <summary>
    /// Scheduler for periodic synchronization
    /// </summary>
    public class SyncScheduler
    {
        private readonly FolderSynchronizer _synchronizer;
        private readonly int _intervalSeconds;
        private Timer _timer;

        public SyncScheduler(FolderSynchronizer synchronizer, int intervalSeconds)
        {
            _synchronizer = synchronizer;
            _intervalSeconds = intervalSeconds;
        }

        /// <summary>
        /// Starts the periodic synchronization
        /// </summary>
        public void Start()
        {
            // Start timer with immediate first execution, then periodic execution
            _timer = new Timer(
                callback: _ => _synchronizer.Synchronize(),
                state: null,
                dueTime: 0,
                period: _intervalSeconds * 1000
            );
        }

        /// <summary>
        /// Stops the periodic synchronization
        /// </summary>
        public void Stop()
        {
            _timer?.Dispose();
        }
    }
}
