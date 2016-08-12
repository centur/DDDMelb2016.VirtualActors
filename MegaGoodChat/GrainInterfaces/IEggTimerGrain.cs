using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IEggTimerGrain : IGrainWithGuidKey
    {
        Task RemindRoom(IChatRoomGrain room, string message, int seconds);
    }
}
