using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Orleans.Storage;

namespace ConsoleSilo
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.Globals.RegisterStorageProvider<OrleansFileStorage>("Default");
            config.Defaults.TraceFilePattern = null;

            // Let the user start multiple silos on different ports.
            var siloNumber = 0;
            if (args.Length > 0) siloNumber = int.Parse(args[0]);
            config.Defaults.Port += siloNumber;

            var siloName = $"Silo {siloNumber}";
            Console.Title = siloName;

            // Start the Orleans silo and wait for it to shutdown.
            var host = new SiloHost(siloName, config);
            host.InitializeOrleansSilo();
            host.StartOrleansSilo();
            host.WaitForOrleansSiloShutdown();
        }
    }
}
