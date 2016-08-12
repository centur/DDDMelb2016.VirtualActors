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
    [StorageProvider(ProviderName = "Default")]
    public class ChatRoomGrain : Grain<ChatRoomState>, IChatRoomGrain
    {
        private readonly Dictionary<string, IChatRoomObserver> users = new Dictionary<string, IChatRoomObserver>();

        public Task JoinRoom(string userName, IChatRoomObserver client)
        {
            users[userName] = client;

            foreach (var messages in this.State.RecentMessages)
            {
                client.OnMessage(messages);
            }

            return Task.FromResult(0);
        }

        public Task LeaveRoom(string userName)
        {
            users.Remove(userName);
            return Task.FromResult(0);
        }

        public Task<List<string>> GetUsers()
        {
            return Task.FromResult(users.Keys.ToList());
        }

        public async Task SendMessage(string userName, string body)
        {
            var message = new ChatMessage
            {
                Body = body,
                RoomName = this.GetPrimaryKeyString(),
                UserName = userName
            };

            // Save the message, but only store recent messages
            if (this.State.RecentMessages.Count > 25) this.State.RecentMessages.Dequeue();
            this.State.RecentMessages.Enqueue(message);
            await this.WriteStateAsync();

            // Send the message to each of the users
            foreach (var user in users.Values)
            {
                user.OnMessage(message);
            }

            // Send the message off to be procesed in some stream.
            await SendMessageToStream(message);
        }

        private async Task SendMessageToStream(ChatMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Body)) return;

            // Pick a stream based on the first character
            // Note: this is some contrived way of fanning out processing of messages.
            var id = CharGuidConverter.GetGuid(message.Body[0]);
            var stream = GetStreamProvider("InMemory").GetStream<ChatMessage>(id, "messages");

            // Push the message to the stream.
            await stream.OnNextAsync(message);
        }
    }

    [Serializable]
    public class ChatRoomState
    {
        public Queue<ChatMessage> RecentMessages { get; set; } = new Queue<ChatMessage>();
    }
}
