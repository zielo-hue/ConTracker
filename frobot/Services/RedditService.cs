using Disqord;
using Disqord.Bot;
using Newtonsoft.Json.Linq;
using Reddit;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace frobot.Services
{
    public class RedditService
    {
        private readonly JObject conf = JObject.Parse(File.ReadAllText(@"config2.json"));
        private readonly string APPID;
        private readonly string REFRESHTOKEN;
        public static RedditAPI r;
        private readonly DiscordBot _bot;
        private readonly Timer _timer;
        private readonly Snowflake channelid = 597875185886035989;
        public RedditService(DiscordBot bot)
        {
            _bot = bot;
            APPID = (string) conf["REDDIT"]["APPID"];
            REFRESHTOKEN = (string) conf["REDDIT"]["REFRESHTOKEN"];
            r = new RedditAPI(APPID, REFRESHTOKEN);
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
