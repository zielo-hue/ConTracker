using Disqord;
using Disqord.Bot;
using Newtonsoft.Json.Linq;
using Reddit;
using System;
using System.IO;
using System.Timers;

namespace frobot.Services
{
    public class RedditService
    {
        private readonly JObject conf = JObject.Parse(File.ReadAllText(@"config.json"));
        private readonly string APPID;
        private readonly string REFRESHTOKEN;
        public RedditAPI r;
        private readonly DiscordBot _bot;
        private readonly Timer _timer;
        private readonly Snowflake channelid = 597875185886035989;
        public RedditService(DiscordBot bot)
        {
            _bot = bot;
            _timer = new Timer();
            _timer.Interval = 30000;
            _timer.Elapsed += UpdateStatus;
            APPID = (string) conf["REDDIT"]["APPID"];
            REFRESHTOKEN = (string)conf["REDDIT"]["REFRESHTOKEN"];
            r = new RedditAPI(APPID, REFRESHTOKEN);
        }

        private async void UpdateStatus(Object source, System.Timers.ElapsedEventArgs e)
        {
            await _bot.SetPresenceAsync(new LocalActivity("bruh", ActivityType.Playing));

        }
    }
}
