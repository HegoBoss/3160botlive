using _3160robot.bot.Handlers.Commands.MultiCommands.Tournament;
using _3160robot.bot.Handlers.Commands.SingleCommands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands
{
    class CommandHandler
    {
        private char prefix;

        private Dictionary<String, ICommand> prefixCommandRegistry = new Dictionary<String, ICommand>();
        private List<ICommand> generalCommandRegistry = new List<ICommand>();


        public CommandHandler(char prefix)
        {
            this.prefix = prefix;

            this.Init();
        }


        private void Init()
        {

            // General commands
            this.AddCommand(new ResponseCommand());

            // Prefix commands
            this.AddCommand("clear", new ClearCommand());
            this.AddCommand("wakeup", new WakeupCommand());
            this.AddCommand("cookie", new CookieClickerCommand());
            this.AddCommand("tourni", new TournamentCommand());

        }

        private void AddCommand(string commandName, ICommand commandHandler)
        {
            prefixCommandRegistry.Add(commandName, commandHandler);
        }


        private void AddCommand(ICommand commandHandler)
        {
            generalCommandRegistry.Add(commandHandler);
        }


        private ICommand? TryGetCommand(string commandName)
        {

            if (prefixCommandRegistry.ContainsKey(commandName))
            {
                return prefixCommandRegistry[commandName];
            }
            else
            {
                return null;
            }

        }


        private void runGeneralCommands(SocketMessage message)
        {
            foreach (ICommand command in generalCommandRegistry)
            {
                command.Execute(message);
            }  
        }

        public void Handle(SocketMessage message)
        {

            bool isPrefixCommand = message.Content.ToLower().StartsWith("!");

            Console.WriteLine(message.Content);

             if (!isPrefixCommand)
             {
                runGeneralCommands(message);
                return;
             }

            string commandRequested = message.Content.Substring(1).ToLower().Split(' ')[0].Trim();

            ICommand? command = TryGetCommand(commandRequested);

            if (command == null)
            {
                return;
            }

            command.Execute(message);

        }

    }
}
