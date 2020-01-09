using Disqord;
using Disqord.Bot;
using Newtonsoft.Json.Linq;
using Reddit;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Disqord.Bot.Sharding;

namespace frobot.Services
{
    public class RedditService
    {
        private readonly JObject _conf = JObject.Parse(File.ReadAllText(@"config2.json"));
        public static RedditAPI R;
        private readonly DiscordBot _bot;
        public RedditService(DiscordBot bot)
        {
            _bot = bot;
            var appid = (string) _conf["REDDIT"]["APPID"];
            var refreshtoken = (string) _conf["REDDIT"]["REFRESHTOKEN"];
            R = new RedditAPI(appid, refreshtoken);
            updatePresence().Start();
        }

        public async Task updatePresence()
        {
            while (true)
            {
                await Task.Delay(30000);
                await _bot.SetPresenceAsync(new LocalActivity("bruh", ActivityType.Playing));
            }
        }
    }
}
