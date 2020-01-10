using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Sharding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steam.Models;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace frobot.Services
{
    public class SteamService
    {
        private static JObject _conf = JObject.Parse(File.ReadAllText(@"config2.json"));
        
        // Temporary
        private readonly DiscordBotSharder _bot;

        public SteamService(DiscordBotSharder bot)
        {
            _bot = bot;
            var webInterfaceFactory = new SteamWebInterfaceFactory((string) _conf["STEAM"]["KEY"]);
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUserStats>(new HttpClient());

            _ = CheckAppVersions(steamInterface);
        }
        
        /*
         * TODO: Wire it up with RedditService, get working SteamVR SteamAPI schema format
         */
        async Task CheckAppVersions(SteamUserStats steamInterface)
        {
            while (true)
            {
                // remember to change this to 1 minute
                await Task.Delay(10000).ConfigureAwait(true);
                
                var gameSchema = steamInterface
                    .GetSchemaForGameAsync(440);
                var gameVersion = gameSchema.Result.Data.GameVersion;

                Console.WriteLine(gameVersion);

                _conf = JObject.Parse(File.ReadAllText(@"config2.json"));
                
                if ((string) _conf["latestAppVersion"] == gameVersion)
                {
                    await _bot.SendMessageAsync(558044111421177866, "app version check, no new changes")
                        .ConfigureAwait(true);
                }
                else if ((string) _conf["latestAppVersion"] != gameVersion)
                {
                    await _bot.SendMessageAsync(558044111421177866, 
                            "app version check, new changes detected: local " + (string) _conf["latestAppVersion"] + " remote "+ gameVersion)
                        .ConfigureAwait(true);
                    _conf["latestAppVersion"] = gameVersion;
                    await File.WriteAllTextAsync(@"config2.json", _conf.ToString()).ConfigureAwait(true);
                }
            }
        }
    }
}