using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Steam.Models;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace frobot.Services
{
    public class SteamService
    {
        private readonly JObject _conf = JObject.Parse(File.ReadAllText(@"config2.json"));

        public SteamService()
        {
            var webInterfaceFactory = new SteamWebInterfaceFactory((string) _conf["STEAM"]["KEY"]);
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUserStats>(new HttpClient());

            _ = CheckAppVersions(steamInterface);
        }
        
        /*
         * TODO: Write check with version in the json and wire it up with RedditService
         */
        async Task CheckAppVersions(SteamUserStats steamInterface)
        {
            var gameSchema = steamInterface
                .GetSchemaForGameAsync(250820, "English");
            var gameVersion = gameSchema.Result.Data.GameVersion;
        }
    }
}