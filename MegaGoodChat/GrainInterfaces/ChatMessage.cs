namespace GrainInterfaces
{
    public class ChatMessage
    {
        public string RoomName { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }

        // TODO: Add a timestamp?
    }
}