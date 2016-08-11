using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace Grains
{
    public class AngryGrain : Grain, IAngryGrain
    {
        public Task<string> ToUpper(string input) => Task.FromResult(input?.ToUpper());
    }
}
