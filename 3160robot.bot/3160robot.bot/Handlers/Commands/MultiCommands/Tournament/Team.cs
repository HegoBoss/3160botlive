using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.MultiCommands.Tournament
{
    class Team
    {

        public int nr;
        private List<SocketUser> users = new List<SocketUser>();

        public Team(int teamId)
        {
            this.nr = teamId;
        }


        public bool HasUser(SocketUser user)
        {
            return this.users.Contains(user);
        }


        public bool AddUser(SocketUser user)
        {
            if (this.HasUser(user))
            {
                return false;
            }

            this.users.Add(user);

            return true;
        }


        public List<SocketUser> GetUsers()
        {
            return this.users;
        }

        public void Clear()
        {
            this.users = new List<SocketUser>();
        }

    }
}
