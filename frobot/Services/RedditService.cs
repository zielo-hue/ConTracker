using Disqord;
using Newtonsoft.Json.Linq;
using Reddit;
using System.IO;
using System.Threading.Tasks;
using Disqord.Bot.Sharding;

namespace frobot.Services
{
    public class RedditService
    {
        private readonly JObject _conf = JObject.Parse(File.ReadAllText(@"config2.json"));
        public static RedditAPI R;
        private readonly DiscordBotSharder _bot;
        public RedditService(DiscordBotSharder bot)
        {
            _bot = bot;
            var appid = (string) _conf["REDDIT"]["APPID"];
            var refreshtoken = (string) _conf["REDDIT"]["REFRESHTOKEN"];
            R = new RedditAPI(appid, refreshtoken);
            _ = UpdatePresence();
        }

        async Task UpdatePresence()
        {
            while (true)
            {
                await Task.Delay(30000);
                await _bot.SetPresenceAsync(new LocalActivity("FUCK holonoid & gaming", ActivityType.Playing));
            }
        }
        
        /*
         * TODO: start working on checking for version changes in the json file
         * TODO: set up steam service
         */
    }
}
