using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace ConsoleClient
{
    class ChatClient :  IChatRoomObserver
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
        {
            // TODO: Use GrainClient.GrainFactory to get a reference to the IChatRoomGrain with the specified id (roomName)
            throw new NotImplementedException("set the currentRoomGrain variable");
        }

        public async Task JoinRoom(string roomName)
        {
            SetCurrentRoom(roomName);
            // TODO: call the grain's JoinRoom method and send it the observerReference so that it can call us
            // back whenever a new message arrives.
            throw new NotImplementedException("Join the room");
        }

        public async Task LeaveRoom()
        {
            if (currentRoomGrain == null)
            {
                Console.WriteLine("Join/switch to a room with /join {room}");
                return;
            }
            
            // TODO: call the LeaveRoom on the room grain.
            throw new NotImplementedException("Leave the room");
            currentRoomGrain = null;
        }

        public async Task SendMessage(string message)
        {
            if (currentRoomGrain == null)
            {
                Console.WriteLine("Join/switch to a room with /join {room}");
                return;
            }

            // TODO: call the SendMessage on the room grain.
            throw new NotImplementedException("Send a message to the room");
        }

        public Task<List<string>> GetUsers() => currentRoomGrain.GetUsers();

        //TODO: Add this method to IChatRoomObserver
        // This method will be called whenever a new message is sent from any of the rooms we are joined to.
        // This is the implementation of IChatRoomObserver
        public void OnMessage(ChatMessage message)
        {
            ConsoleLog.OnMessage(message);
            ConsoleLog.WritePrompt(this.RoomName, this.UserName);
        }
    }
}
