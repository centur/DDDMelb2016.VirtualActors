using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IStatisticsCollectorGrain : IGrainWithIntegerKey
    {
        Task ReportStats(Dictionary<char, int> usages);
        Task<Dictionary<char, int>> GetStats();
    }
}
