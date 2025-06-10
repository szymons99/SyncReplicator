using System;
using SyncReplicator.Core;
using SyncReplicator.Utils;

namespace SyncReplicator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Validate and parse command line arguments
            var config = ArgumentValidator.ValidateAndParse(args);
            if (config == null) return;

            // Create and start the synchronizer
            var synchronizer = new FolderSynchronizer(config);
            var scheduler = new SyncScheduler(synchronizer, config.IntervalSeconds);

            Console.WriteLine($"Starting synchronization every {config.IntervalSeconds} seconds...");
            Console.WriteLine("Press 'q' to quit the program.");

            scheduler.Start();

            // Wait for user to quit
            while (Console.ReadKey().KeyChar != 'q') { }

            scheduler.Stop();
            Console.WriteLine("\nProgram terminated.");
        }
    }
}
