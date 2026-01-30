using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands
{
    abstract class MultiCommand : ICommand
    {   

        private Dictionary<string, Action<SocketMessage>> internalCommands = new Dictionary<string, Action<SocketMessage>>();


        public MultiCommand()
        {

            this.AddInternalCommand("help", this.HelpCommand);
            this.Init();

        }

        abstract protected void Init();

        protected async void HelpCommand(SocketMessage message)
        {

            int commandAmount = this.internalCommands.Count;

            string baseCommand = message.Content.Substring(1).Split(' ')[0].Trim().ToLower();

            if (commandAmount <= 1)
            {
                return;
            }

            string responseMessage = $"The {baseCommand} command has the following sub commands:";

            foreach (string commandName in internalCommands.Keys)
            {
                responseMessage += $"\n- {commandName}";
            }

            await message.Channel.SendMessageAsync(responseMessage);

        }

        protected void RunInternalCommand(SocketMessage message)
        {
            string receivedCommand = message.Content.Split(' ')[1].Trim().ToLower();

            Action<SocketMessage>? action = this.TryGetInternalCommand(receivedCommand);

            if (action == null)
            {
                return;
            }

            action(message);

        }

        protected void AddInternalCommand(string commandName, Action<SocketMessage> function)
        {
            internalCommands[commandName] = function;
        }

        protected Action<SocketMessage>? TryGetInternalCommand(string commandName)
        {
            if (internalCommands.ContainsKey(commandName))
            {
                return internalCommands[commandName];
            }
            else
            {
                return null;
            }
        }

        public async Task Execute(SocketMessage message)
        {
            this.RunInternalCommand(message);
        }
    }
}
