using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands
{
    interface ICommand
    {
        Task Execute(SocketMessage message);
    }
}
