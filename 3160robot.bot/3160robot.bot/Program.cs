using _3160robot.bot.Handlers.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace testbot.cons
{
    internal class Program
    {
        private readonly DiscordSocketClient _client;
        private CommandHandler commandHandler;

        public Program()
        {
            DiscordSocketConfig config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            this.commandHandler = new CommandHandler('!');

            _client = new DiscordSocketClient(config);
            _client.MessageReceived += MessageHandler;
            _client.Log += LogFuncAsync;
        }

        public static void Main(string[] args) => new Program().StartBotAsync().GetAwaiter().GetResult();

        public async Task StartBotAsync()
        {
            string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("LET OP: Geen DISCORD_TOKEN gevonden.");
                return;
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task LogFuncAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            await Task.CompletedTask;
        }

        private async Task MessageHandler(SocketMessage message)
        {
            if (message.Author.IsBot || message.Channel is not SocketTextChannel) return;

            commandHandler.Handle(message);
        }
    }
}