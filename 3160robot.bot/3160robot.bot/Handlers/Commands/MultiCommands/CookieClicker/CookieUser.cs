using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.MultiCommands.CookieClicker
{
    public class CookieUser
    {
        private ulong userId;
        private int cookies;
        private int clickAmout = 1;

        public CookieUser(ulong userId) {
        
            this.userId = userId;
        }


        public ulong GetUserId()
        {
            return this.userId;
        }

        public int GetCookies()
        {
            return this.cookies;
        }

        public int GetClickAmount()
        {
            return this.clickAmout;
        }

        public void Click()
        {
            this.cookies += this.clickAmout;
        }
    }
}
