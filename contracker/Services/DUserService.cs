using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot.Sharding;
using Qmmands;

namespace contracker.Services
{
    public class DUserService
    {
        private DiscordClient client;
        public DBotService DBotService { get; set; }
        public SteamService SteamService { get; set; }
        
        public DUserService(string token)
        {
            client = new DiscordClient(TokenType.User, token);
            _ = UpdatePresence();
        }
        
        async Task UpdatePresence()
        {
            while (true)
            {
                await Task.Delay(30000);
                await DBotService._bot.SetPresenceAsync(new LocalActivity("FUCK holonoid & gaming", ActivityType.Playing));
            }
        }

        // Use steam API
        public async Task<List<string>> GetAccounts(Snowflake userId)
        {
            var profile = await client.GetProfileAsync(userId: userId);
            List<string> steamAccounts = new List<string>();
            foreach (var account in profile.ConnectedAccounts)
            {
                if(account.Type == "steam") steamAccounts.Add(account.Id);
            }
            return steamAccounts;
        }

        public string GetSteamName(string id)
        {
            return SteamService.steamUser.GetPlayerSummaries(steamids: id)["players"][0]["personaname"].AsString();
        }
        
        public async Task<bool> IsVerified(Snowflake userId)
        {
            var profile = await client.GetProfileAsync(userId: userId);
            bool verified = false;
            foreach (var account in profile.ConnectedAccounts)
            {
                if (account.Type == "steam" && account.IsVerified == true)
                    verified = true;
                else
                    verified = false;
            }
            return verified;
        }
        
        /*// Legacy
        public async Task<List<string>> GetSteamNames(List<string> ids)
        {
            List<string> steamNames = new List<string>();
            foreach (var id in ids)
            {
                steamNames.Add(SteamService.steamUser.GetPlayerSummaries(steamids: id)["players"][0]["personaname"].AsString());
            }
            return steamNames;
        }*/
    }
}