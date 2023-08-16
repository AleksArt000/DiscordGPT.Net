using Discord;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BasicBot
{
    // This is a bot based on a minimal, bare-bones example of using Discord.Net.

    class Program
    {
        System.String token = "token";
        System.String ID = "<@" + "id" + ">";
        System.String pythonPath = "path-to-python3.11.exe";

        // Non-static readonly fields can only be assigned in a constructor.
        // If you want to assign it elsewhere, consider removing the readonly keyword.
        private readonly DiscordSocketClient _client;

        // Discord.Net heavily utilizes TAP for async, so we create
        // an asynchronous context from the beginning.
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public Program()
        {
            // Config used by DiscordSocketClient
            // Define intents for the client
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            // It is recommended to Dispose of a client when you are finished
            // using it, at the end of your app's lifetime.
            _client = new DiscordSocketClient(config);

            // Subscribing to client events, so that we may receive them whenever they're invoked.
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;        }

        public async Task MainAsync()
        {
            // Tokens should be considered secret data, and never hard-coded (unless you're lazy and can't be bothered to).
            await _client.LoginAsync(TokenType.Bot, token);
            Console.WriteLine(token);
            // Different approaches to making your token a secret is by putting them in local .json, .yaml, .xml or .txt files, then reading them on startup (complicated).

            await _client.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a
        // connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        // This is not the recommended way to write a bot (🥱) - consider
        // reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            switch(message.Content) 
            {
             case "!ping":
                // code block

                // Send a message with content 'pong', including a button.
                await message.Channel.SendMessageAsync("pong!");
                break;
            default:
                // code block
                if (message.Content.StartsWith(ID))
                {   

                    System.String path = "/gpt4all-client/client.py";
                    System.String messageContent = "";

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = pythonPath,
                            Arguments = path + " " + message.Content,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = false,
                            CreateNoWindow = true
                        },
                        EnableRaisingEvents = true
                    };

                    using (message.Channel.EnterTypingState())
                    {
                        process.Start();

                        while (!process.StandardOutput.EndOfStream)
                        {
                            var Data = process.StandardOutput.ReadLine(); 
                              if( Data != null)
                              {
                                messageContent = messageContent + Data;
                              }
                        }
                        process.Close();

                        if (messageContent == "")
                        {
                            messageContent = "...";
                        }
                        MessageReference reference = new MessageReference(message.Id);
                        await message.Channel.SendMessageAsync(messageContent, messageReference:reference);
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
