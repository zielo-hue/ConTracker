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
        
        public async Task<string> GetSteamAccount(Snowflake userId)
        {
            var profile = await client.GetProfileAsync(userId: userId);
            string steamAccount = "None";
            foreach (var account in profile.ConnectedAccounts)
            {
                if(account.Type == "steam") steamAccount = (account.Id);
            }
            return steamAccount;
        }
        
        public async Task<bool> IsVerified(Snowflake userId)
        {
            var profile = await client.GetProfileAsync(userId: userId);
            if (profile.ConnectedAccounts.First().IsVerified == true && profile.ConnectedAccounts.First().Type.Equals("steam"))
                return true;
            else
                return false;
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