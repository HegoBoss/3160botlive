using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.SingleCommands
{
    public class ClearCommand : SingleCommand
    {
        public override async Task Execute(SocketMessage message)
        {
            SocketGuildUser guildUser = (SocketGuildUser) message.Author;

            if (guildUser == null || !guildUser.GuildPermissions.ManageMessages)
            {
                await message.Channel.SendMessageAsync("⛔ Jij mag geen berichten verwijderen!");
                return;
            }


            var parts = message.Content.Split(' ');


            Console.WriteLine(parts);

            if (parts.Length < 2)
            {
                await message.Channel.SendMessageAsync("⚠️ Geef op hoeveel berichten weg moeten. Bijv: `clear 5`");
                return;
            }

              


            if (int.TryParse(parts[1], out int amount))
            {

                if (amount > 100) amount = 100;
                if (amount < 1) amount = 1;


                var channel = message.Channel as ITextChannel;


                var messages = await channel.GetMessagesAsync(amount + 1).FlattenAsync();


                var validMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays < 14);

                await channel.DeleteMessagesAsync(validMessages);


                var confirmMsg = await channel.SendMessageAsync($"🧹 {amount} berichten verwijderd!");
                await Task.Delay(3000);
                await confirmMsg.DeleteAsync();
            }
            else
            {
                await message.Channel.SendMessageAsync("⚠️ Dat is geen geldig getal.");
            }
        }
    }
}