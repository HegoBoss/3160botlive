using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.SingleCommands
{
    public class ResponseCommand : SingleCommand
    {
        // De lijst met woorden en antwoorden
        private static readonly Dictionary<string, List<string>> _responses = new()
        {
            { "hi", new List<string> { "Hello There", "Hoi!", "Goeiedag even", "Yo maat!", "Ewa!" } },
            { "schuld", new List<string> { "Altijd de schuld van Kenny!", "Kenny heeft het weer gedaan...", "Waarom altijd Kenny?", "100% Kenny." } },
            { "test", new List<string> { "Ik werk prima.", "Systeem operationeel.", "Beep boop." } },
            { "homo", new List<string> { "niet schelden aub", "gelieve niet zo een woordenschat te gebruiken", "je zie zelve homo!" } }
        };

        public override async Task Execute(SocketMessage message)
        {
            var content = message.Content;
            var random = new Random();

            foreach (var entry in _responses)
            {
                if (Regex.IsMatch(content, $@"\b{Regex.Escape(entry.Key)}\b", RegexOptions.IgnoreCase))
                {
                    var mogelijkeAntwoorden = entry.Value;
                    string antwoord = mogelijkeAntwoorden[random.Next(mogelijkeAntwoorden.Count)];
                    antwoord += $" {message.Author.Mention}";

                    await message.Channel.SendMessageAsync(antwoord);
                }
            }
        }
    }
}