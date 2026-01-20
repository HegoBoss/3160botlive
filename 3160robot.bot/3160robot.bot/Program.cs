using Discord;
using Discord.WebSocket;

namespace _3160robot.bot
{
    internal class Program
    {
        private readonly DiscordSocketClient client;
        

        public Program()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            this.client = new DiscordSocketClient(config);
            this.client.MessageReceived += MessageHandler;
        }
        public async Task StartBotAsync()
        {
            string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            this.client.Log += LogFuncAsync;
            await this.client.LoginAsync(TokenType.Bot, token);
            await this.client.StartAsync();
            await Task.Delay(-1);

            async Task LogFuncAsync(LogMessage message) =>
                Console.Write(message.ToString());

            //await this.client.LoginAsync(Discord.TokenType.Bot, token);
        }

        private async Task MessageHandler(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            string tekst = message.Content.ToLower();

            if (message.Content.Contains("Hello There"))
            {
                await message.Channel.SendMessageAsync("General Kenobi! You are a bold one.");
            }
            else if (message.Content.Contains("hi")|| message.Content.Contains("hello") || message.Content.Contains("sup")|| message.Content.Contains("gekleurd"))
            {
                await message.Channel.SendMessageAsync("Hello there my name is 3160 whats your name");
            }
            else if (message.Content.Contains("check"))
            {
                await message.Channel.SendMessageAsync("Beep boop. Systems operational. Hello, user.");
            }

            //await ReplyAsync(message, "Greetings! I am ready to assist. (Or just chat, mostly chat).");
        }

        private async Task ReplyAsync(SocketMessage message, string response) =>
            await message.Channel.SendMessageAsync(response);

        static void Main(string[] args) => new Program().StartBotAsync().GetAwaiter().GetResult();
    }
}
