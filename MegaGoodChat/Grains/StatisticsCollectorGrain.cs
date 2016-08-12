using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Providers;

namespace Grains
{ 
    public class StatCollectorGrain : Grain, IStatisticsCollectorGrain
    {
        private readonly Dictionary<char, int> freqs = new Dictionary<char, int>();
        public Task ReportStats(Dictionary<char, int> usages)
        {
            foreach (var val in usages)
            {
                AddUsages(val.Key, val.Value);
            }

            return Task.FromResult(0);
        }

        private void AddUsages(char character, int usages)
        {
            int initialValue;
            freqs.TryGetValue(character, out initialValue);
            freqs[character] = initialValue + usages;
        }

        public Task<Dictionary<char, int>> GetStats() => Task.FromResult(freqs);
    }
}
