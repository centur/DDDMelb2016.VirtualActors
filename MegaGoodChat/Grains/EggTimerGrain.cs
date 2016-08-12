using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;

namespace Grains
{
    // TODO: Implement IRemindable so that the grain can receive reminders!
    [StorageProvider(ProviderName = "Default")]
    public class EggTimerGrain : Grain<EggTimerState> //TODO: , IEggTimerGrain
    {
        // TODO: Write some state so we know what to do when the reminder fires
        // TODO: Register a reminder
    }

    [Serializable]
    public class EggTimerState
    {
        public IChatRoomGrain Room { get; set; }
        public string Message { get; set; }
    }
}
