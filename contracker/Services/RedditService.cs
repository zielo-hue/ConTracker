using Disqord;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using Disqord.Bot.Sharding;

namespace contracker.Services
{
    public class RedditService
    {
        private readonly JObject _conf = JObject.Parse(File.ReadAllText(@"config.json"));
        private readonly JObject _confChungus = JObject.Parse(File.ReadAllText(@"config.json"));
        private readonly DiscordBotSharder _bot;
        public RedditService(DiscordBotSharder bot)
        {
            _bot = bot;
            var appid = (string) _conf["REDDIT"]["APPID"];
            var refreshtoken = (string) _conf["REDDIT"]["REFRESHTOKEN"];
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
    }
}
