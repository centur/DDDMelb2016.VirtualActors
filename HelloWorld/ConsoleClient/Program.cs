using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Runtime.Configuration;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception}");
                throw;
            }
        }

        static async Task MainAsync(string[] args)
        {
            // Create a configuration with the default local silo settings (ports, etc) & connect to it.
            var config = ClientConfiguration.LocalhostSilo();
            GrainClient.Initialize(config);

            // Use the client to communicate with grains.
            while (true)
            {
                var cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd)) break;


                Console.WriteLine(result);
            }
        }
    }
}
