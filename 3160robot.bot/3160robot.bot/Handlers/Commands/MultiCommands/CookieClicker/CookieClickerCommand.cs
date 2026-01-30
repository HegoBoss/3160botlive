using _3160robot.bot.Handlers.Commands.MultiCommands.CookieClicker;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3160robot.bot.Handlers.Commands.SingleCommands
{
    class CookieClickerCommand : MultiCommand
    {

        private Dictionary<ulong, CookieUser> cookieStore = new Dictionary<ulong, CookieUser>();




        protected override void Init()
        {
            this.AddInternalCommand("click", this.AddCookie);
            this.AddInternalCommand("show", this.ShowCookie);
        }

        private async void AddCookie(SocketMessage message)
        {
            ulong userId = message.Author.Id;

            CookieUser cookieUser;

            if (!cookieStore.ContainsKey(userId))
            {
                cookieUser = new CookieUser(userId); 
            } 
            else
            {
                cookieUser = cookieStore[userId];
            }


            cookieUser.Click();

            cookieStore[userId] = cookieUser;

            await message.Channel.SendMessageAsync($"{message.Author.Mention} has gained {cookieUser.GetClickAmount()} cookie(s)!");
        }


        private async void ShowCookie(SocketMessage message)
        {
            ulong userId = message.Author.Id;

            if (!cookieStore.ContainsKey(userId))
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention} has no cookies!");
            } else
            {
                int cookies = cookieStore[userId].GetCookies();
                await message.Channel.SendMessageAsync($"{message.Author.Mention} has {cookies} cookies!");
            }
        }
    }
}
