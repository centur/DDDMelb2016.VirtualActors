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
        Task JoinRoom(string userName, IChatRoomObserver client);
        Task LeaveRoom(string userName);
        Task<List<string>> GetUsers();
        Task SendMessage(string userName, string body);
    }
}
