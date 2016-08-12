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

    // This is the implementation of our chat room.
    
    // Since we're here, note two things:
    // 1. We add the StorageProvider attribute to tell Orleans where out state belongs.
    //    "Default" corresponds to the provider configured in ConsoleSilo\Program.cs
    // 2. Instead of inheriting from Grain as we did in the HelloWorld example, we inherit
    //    From Grain<TState> where TState is the class which holds our grain's persisted state.
    //    Orleans will load this state for us BEFORE any method is called.
    [StorageProvider(ProviderName = "Default")]
    public class ChatRoomGrain : Grain<ChatRoomState>, IChatRoomGrain
    {
        // keep an in-memory dictionary of users and their corresponding observers.
        private readonly Dictionary<string, IChatRoomObserver> users = new Dictionary<string, IChatRoomObserver>();

        public Task JoinRoom(string userName, IChatRoomObserver client)
        {
            users[userName] = client;

            // TODO: Send each of the recent messages (from this.State.RecentMessages or something) to the new client.

            return Task.FromResult(0);
        }

        public Task LeaveRoom(string userName)
        {
            users.Remove(userName);
            return Task.FromResult(0);
        }

        // A lot of your methods will be simple one-liners like this.
        public Task<List<string>> GetUsers() => Task.FromResult(users.Keys.ToList());

        // TODO: this method should be on the grain interface so clients can call it!
        public async Task SendMessage(string userName, string body)
        {
            var message = new ChatMessage
            {
                Body = body,
                RoomName = this.GetPrimaryKeyString(),
                UserName = userName
            };

            // TODO:  Uncomment the following lines to update the grain's state.
            // Note that the state is automatically loaded for us - we don't do anything.

            // Save the message, but only store recent messages
            //if (this.State.RecentMessages.Count > 25) this.State.RecentMessages.Dequeue();
            //this.State.RecentMessages.Enqueue(message);
            // TODO: Write the state so that messages are saved between crashes

            // TODO: Send the message to each of the users
        }
    }
    [Serializable]
    public class ChatRoomState
    {
        // We are using a queue because it's a simple way to store the most recent N messages
        public Queue<ChatMessage> RecentMessages { get; set; } = new Queue<ChatMessage>();

        // TODO: What else might be interesting to store here? Room admin? Secret (hashed) password?
    }
}
