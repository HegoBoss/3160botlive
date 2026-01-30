using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.MultiCommands.Tournament
{
    class TournamentCommand : MultiCommand
    {

        private Dictionary<ulong, List<Team>> teams = new Dictionary<ulong, List<Team>>();
        private int defaultTeamsAmount = 2;

        private Dictionary<ulong, List<SocketVoiceChannel>> createdChannels = new Dictionary<ulong, List<SocketVoiceChannel>>();

        private bool isStarted = false;
        private Dictionary<ulong, SocketVoiceChannel?> startChannels = new Dictionary<ulong, SocketVoiceChannel?>();


        protected override void Init()
        {
            this.AddInternalCommand("clear", this.ClearCommand);
            this.AddInternalCommand("create", this.CreateTeamsCommand);
            this.AddInternalCommand("start", this.StartCommand);
            this.AddInternalCommand("stop", this.StopCommand);
            this.AddInternalCommand("show", this.ShowCommand);
        }

        private void ClearTeams(ulong guildId)
        {
            teams[guildId] = new List<Team>();
        }



        private void CreateTeams(ulong guildId, List<SocketUser> userList, int amount = 2)
        {
            if (amount <= 1)
            {
                return;
            }

            

            this.ClearTeams(guildId);

            for (int i = 0; i < amount; i++)
            {
                this.teams[guildId].Add(new Team(i + 1));
            }


            int chosenTeam = 0;

            Random rnd = new Random();

            List<SocketUser> shuffled = userList.OrderBy(x => rnd.Next()).ToList();

            foreach (SocketGuildUser user in shuffled)
            {
                this.teams[guildId][chosenTeam].AddUser(user);

                chosenTeam = (chosenTeam + 1) % this.teams[guildId].Count;
            }

        }

        private int GetTeamsAmount(ulong guildId)
        {
            return this.teams.Count;
        }


        private string ShowTeamsMessage(ulong guildId)
        {

            if (this.GetTeamsAmount(guildId) <= 0)
            {
                return "No teams have been created yet!";
            }

            string message = "These are the teams:\n";

            foreach (Team team in this.teams[guildId])
            {
                message += $"\nTeam {team.nr}\n";

                foreach (SocketGuildUser user in team.GetUsers())
                {
                    message += $"- {user.DisplayName}\n";
                }

                message += "\n";
            }

            return message;
        }


        private SocketGuild GetGuildFromMessage(SocketMessage message)
        {
            return ((SocketTextChannel)message.Channel).Guild;
        }

        private async void ShowCommand(SocketMessage message)
        {
            ulong guildId = this.GetGuildFromMessage(message).Id;

            if (this.GetTeamsAmount(guildId) <= 0)
            {
                await message.Channel.SendMessageAsync("No teams created yet!");
                return;
            }

            await message.Channel.SendMessageAsync(this.ShowTeamsMessage(guildId));
        }

        private async void StartCommand(SocketMessage message)
        {
            ulong guildId = this.GetGuildFromMessage(message).Id;

            if (!this.createdChannels.ContainsKey(guildId))
            {
                this.createdChannels[guildId] = new List<SocketVoiceChannel>();
            }

            if (this.GetTeamsAmount(guildId) <= 0)
            {
                await message.Channel.SendMessageAsync("No teams have been created yet!\nUse the '!tourni create' command while in a voice channel with people you want to play.");
                return;
            }

            if (this.isStarted)
            {
                await message.Channel.SendMessageAsync("Tourni already started!\nUse '!tourni stop' to stop it.");
                return;
            }

            this.isStarted = true;

            string responseMessage = "Tourni started!";



            await message.Channel.SendMessageAsync(responseMessage);
  
            SocketGuild currentGuild = GetGuildFromMessage(message);

            foreach (Team team in this.teams[guildId])
            {
                string voiceChannelName = $"Tourni - Team {team.nr}";

                SocketVoiceChannel? voiceChannel = currentGuild.VoiceChannels
                    .FirstOrDefault(vc => vc.Name.Equals(voiceChannelName, System.StringComparison.OrdinalIgnoreCase));


                if (voiceChannel == null)
                {
                    RestVoiceChannel tourniChannel = await currentGuild.CreateVoiceChannelAsync(voiceChannelName);
                    voiceChannel = currentGuild.GetVoiceChannel(tourniChannel.Id);
                }

                this.createdChannels[guildId].Add(voiceChannel);

                foreach(SocketGuildUser user in team.GetUsers())
                {
                    try
                    {
                        await user.ModifyAsync(x => x.Channel = voiceChannel);
                    } 
                    catch{}

                }

            }

        }

        private async void StopCommand(SocketMessage message)
        {
            if (!this.isStarted)
            {
                await message.Channel.SendMessageAsync("Tourni has not been started yet!");
                return;
            }

            await message.Channel.SendMessageAsync("Tourni Done!");

            ulong guildId = this.GetGuildFromMessage(message).Id;

            foreach (Team team in this.teams[guildId])
            {

                foreach (SocketGuildUser user in team.GetUsers())
                {
                    try
                    {
                        await user.ModifyAsync(x => x.Channel = this.startChannels[guildId]);
                    }
                    catch { }

                }

            }

            foreach(SocketVoiceChannel channel in createdChannels[guildId])
            {
                await channel.DeleteAsync();
            }

            this.createdChannels[guildId] = new List<SocketVoiceChannel>();

            this.isStarted = false;
        }

        private async void ClearCommand(SocketMessage message)
        {
            if (this.isStarted)
            {
                await message.Channel.SendMessageAsync("Tourni already started!\nUse '!tourni stop' to stop it.");
                return;
            }

            ulong guildId = this.GetGuildFromMessage(message).Id;

            this.ClearTeams(guildId);

            await message.Channel.SendMessageAsync("Teams have been cleared!");
        }


        private async void CreateTeamsCommand(SocketMessage message)
        {

            if (this.isStarted)
            {
                await message.Channel.SendMessageAsync("Tourni already started!\nUse '!tourni stop' to stop it.");
                return;
            }

            SocketGuildUser currentUser = (SocketGuildUser)message.Author;

            SocketVoiceChannel channel = currentUser.VoiceChannel;

            if (channel == null)
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention} you are currently not connected to a voice channel!");
                return;
            }

            List<SocketUser> connectedUsers = channel.ConnectedUsers.ToList<SocketUser>();

            string[] commandArgs = message.Content.Split(' ');

            int teamAmount = this.defaultTeamsAmount;

            if (commandArgs.Length >= 3)
            {
                int.TryParse(commandArgs[2], out teamAmount);

                if(teamAmount <= 1)
                {
                    await message.Channel.SendMessageAsync("You need to create more than 1 team!");
                    return;
                }
                else if (teamAmount > connectedUsers.Count)
                {
                    await message.Channel.SendMessageAsync($"You do not have enough players to create {teamAmount} teams!\nYou only have {connectedUsers.Count} players.");
                    return;
                }
 
            }

            ulong guildId = this.GetGuildFromMessage(message).Id;

            this.startChannels[guildId] = channel;

            this.CreateTeams(guildId, connectedUsers, teamAmount);

            await message.Channel.SendMessageAsync(this.ShowTeamsMessage(guildId));

        }

    }
}
