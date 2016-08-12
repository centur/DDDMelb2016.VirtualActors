using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace ConsoleClient
{
    class ChatClient : IChatRoomObserver
    {
        private readonly IChatRoomObserver observerReference;
        private IChatRoomGrain currentRoomGrain;

        public ChatClient()
        {
            // When using IGrainObserver's, we need to register our implementation with Orleans.
            // Orleans will give us back a reference object which we can send to grains.
            this.observerReference = GrainClient.GrainFactory.CreateObjectReference<IChatRoomObserver>(this).Result;
        }

        public string UserName { get; set; }

        // We are identifing chat room grains using their name.
        public string RoomName => currentRoomGrain?.GetPrimaryKeyString() ?? "no room";

        // Get the grain based on the 
        public void SetCurrentRoom(string roomName)
            => currentRoomGrain = GrainClient.GrainFactory.GetGrain<IChatRoomGrain>(roomName);

        public async Task JoinRoom(string roomName)
        {
            SetCurrentRoom(roomName);
            await currentRoomGrain.JoinRoom(UserName, observerReference);
        }

        public async Task LeaveRoom()
        {
            if (currentRoomGrain == null)
            {
                Console.WriteLine("Join/switch to a room with /join {room}");
                return;
            }

            await currentRoomGrain.LeaveRoom(UserName);
            currentRoomGrain = null;
        }

        public async Task SetTimer(string message, int seconds)
        {
            if (currentRoomGrain == null)
            {
                Console.WriteLine("Join/switch to a room with /join {room}");
                return;
            }

            // TODO: Get a random IEggTimerGrain and use it to set a reminder.
        }

        public async Task SendMessage(string message)
        {
            if (currentRoomGrain == null)
            {
                Console.WriteLine("Join/switch to a room with /join {room}");
                return;
            }

            await currentRoomGrain.SendMessage(UserName, message);
        }

        public Task<List<string>> GetUsers() => currentRoomGrain.GetUsers();

        // This method will be called whenever a new message is sent from any of the rooms we are joined to.
        // This is the implementation of IChatRoomObserver
        public void OnMessage(ChatMessage message)
        {
            ConsoleLog.OnMessage(message);
            ConsoleLog.WritePrompt(this.RoomName, this.UserName);
        }
    }
}
