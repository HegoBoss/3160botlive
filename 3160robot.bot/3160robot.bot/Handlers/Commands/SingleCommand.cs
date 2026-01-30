using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands
{
    abstract public class SingleCommand : ICommand
    {
        abstract public Task Execute(SocketMessage message);

    }
}
