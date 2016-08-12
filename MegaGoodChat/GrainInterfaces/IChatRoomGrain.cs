using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IChatRoomGrain : IGrainWithStringKey
    {
        // TODO: Add a JoinRoom method which takes a username and an IChatRoomObserver.
        // The observer will receive all recent messages and all new messages.

        Task LeaveRoom(string userName);
        Task<List<string>> GetUsers();

        // TODO: Add a SendMessage method which takes a username & message, then broadcasts that message to all observers.
    }
}
