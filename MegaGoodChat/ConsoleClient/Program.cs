using System;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime.Configuration;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync().Wait();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception}");
                throw;
            }
        }

        static async Task MainAsync()
        {
            // Create a configuration with the default local silo settings (ports, etc) & connect to it.
            var config = ClientConfiguration.LocalhostSilo();
            config.TraceFileName = null; // Don't trace to file.
            GrainClient.Initialize(config);

            Console.WriteLine("\n"
                              + "==================================\n"
                              + "==    Welcome to MegaGoodChat!  ==\n"
                              + "==================================\n");
            Usage();

            // Use the client to communicate with grains.
            var client = new ChatClient();
            client.UserName = $"user_{new Random().Next(1,25)}";
            while (true)
            {
                try
                {
                    // Write out a command prompt
                    ConsoleLog.WritePrompt(client.RoomName, client.UserName);

                    // Read user input
                    var cmd = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(cmd)) break;

                    // Handle the different commands
                    if (cmd.StartsWith("/"))
                    {
                        var tokens = cmd.Split(' ');
                        if (tokens[0] == "/join")
                        {
                            ConsoleLog.LogCommand("join", tokens[1]);
                            await client.JoinRoom(tokens[1]);
                        }
                        else if (tokens[0] == "/leave")
                        {
                            ConsoleLog.LogCommand("leave", null);
                            await client.LeaveRoom();
                        }
                        else if (tokens[0] == "/users")
                        {
                            ConsoleLog.LogCommand("users", null);
                            var users = await client.GetUsers();
                            Console.WriteLine("\t" + string.Join("\n\t", users));
                        }
                        else if (tokens[0] == "/egg")
                        {
                            var seconds = int.Parse(tokens[1]);
                            var message = string.Join(" ", tokens.Skip(2));
                            ConsoleLog.LogCommand("set reminder", $"in {seconds}s, send \"{message}\" to {client.RoomName}");
                            await client.SetTimer(message, seconds);
                        }
                        else
                        {
                            ConsoleLog.LogError($"Unknown command, \"{tokens[0]}\"");
                            Usage();
                        }
                    }
                    else
                    {
                        // Send the user's input as a message to the current room
                        await client.SendMessage(cmd);
                    }
                }
                catch (Exception e)
                {
                    ConsoleLog.LogError(e.ToString());
                }
            }
        }

        static void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("/join {room}\tJoins or switches to a room");
            Console.WriteLine("/leave\tLeaves the current room");
            Console.WriteLine("{message}\tSends a message to the current room");
            Console.WriteLine("/users\tList users in the current room");
            Console.WriteLine("/egg {seconds} {message}\tSet an egg timer for the current room.");
        }
    }
}
