using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace ConsoleClient
{
    public static class ConsoleLog
    {
        private static readonly object consoleLock = new object();

        // This method will be called whenever a new message is sent from any of the rooms we are joined to.
        // This is the implementation of IChatRoomObserver
        public static void OnMessage(ChatMessage message)
        {
            lock (consoleLock)
            {
                // Print the message using fancy colors.
                Console.ResetColor();
                Console.Write("\r");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"[{message.RoomName}] ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"@{message.UserName}> ");
                Console.ResetColor();
                Console.WriteLine(message.Body);
            }
        }

        public static void WritePrompt(string roomName, string userName)
        {
            lock (consoleLock)
            {
                Console.ResetColor();
                Console.Write("\r");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(userName);
                Console.ResetColor();
                Console.Write("@");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(roomName);
                Console.ResetColor();
                Console.Write("> ");
            }
        }


        public static void LogCommand(string commandName, string parameter)
        {
            lock (consoleLock)
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"<{commandName}> ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(parameter);
                Console.ResetColor();
            }
        }

        public static void LogError(string text)
        {
            lock (consoleLock)
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(text);
                Console.ResetColor();
            }
        }
    }
}
