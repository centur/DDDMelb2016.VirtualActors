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
    [StorageProvider(ProviderName = "Default")]
    public class EggTimerGrain : Grain<EggTimerState>, IEggTimerGrain, IRemindable
    {
        public async Task RemindRoom(IChatRoomGrain room, string message, int seconds)
        {
            this.State.Message = message;
            this.State.Room = room;
            await this.WriteStateAsync();

            // Create the reminder.
            await this.RegisterOrUpdateReminder("timer_expired", TimeSpan.FromSeconds(seconds), TimeSpan.FromMinutes(1));
        }
        
        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            await this.State.Room.SendMessage($"EggTimer_{this.GetPrimaryKey()}", this.State.Message);

            // Unregister the reminder.
            var reminder = await this.GetReminder(reminderName);
            await this.UnregisterReminder(reminder);
        }
    }

    [Serializable]
    public class EggTimerState
    {
        public IChatRoomGrain Room { get; set; }
        public string Message { get; set; }
    }
}
