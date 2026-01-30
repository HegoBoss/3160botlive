using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.SingleCommands
{
    public class WakeupCommand : SingleCommand
    {
        public override async Task Execute(SocketMessage message)
        {
            // 1. Permissie Check
            var guildUser = message.Author as SocketGuildUser;
            if (guildUser == null || !guildUser.GuildPermissions.Administrator && !guildUser.GuildPermissions.MoveMembers)
            {
                await message.Channel.SendMessageAsync("⛔ Jij hebt geen rechten om de Wakeup call te gebruiken!");
                return;
            }

            // 2. Logica
            var targetUser = message.MentionedUsers.First() as IGuildUser;

            if (targetUser?.VoiceChannel == null)
            {
                await message.Channel.SendMessageAsync("Die persoon zit niet in een voice channel!");
                return;
            }

            var originalChannel = targetUser.VoiceChannel;
            var guild = (message.Channel as SocketGuildChannel)?.Guild;
            var otherChannels = guild.VoiceChannels.Where(c => c.Id != originalChannel.Id).ToList();

            if (otherChannels.Count == 0)
            {
                await message.Channel.SendMessageAsync("Er zijn geen andere kanalen om hem naartoe te gooien!");
                return;
            }

            await message.Channel.SendMessageAsync($"🚑 WAKE UP CALL VOOR {targetUser.Mention}!");

            var random = new Random();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    if (targetUser.VoiceChannel == null) break;

                    var randomChannel = otherChannels[random.Next(otherChannels.Count)];
                    await targetUser.ModifyAsync(x => x.Channel = randomChannel);
                    await Task.Delay(1000);
                }

                if (targetUser.VoiceChannel != null)
                {
                    await targetUser.ModifyAsync(x => x.Channel = new Optional<IVoiceChannel>(originalChannel));
                    await message.Channel.SendMessageAsync("Hij is weer veilig terug. Grapje geslaagd.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout tijdens wakeup call: {ex.Message}");
                await message.Channel.SendMessageAsync("Oei, er ging iets mis (heb ik wel 'Move Members' rechten?).");
            }
        }
    }
}