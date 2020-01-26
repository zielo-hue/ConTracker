using System;
using SteamKit2;

namespace contracker.Services
{
    public class SteamService
    {
        public dynamic steamUser;
        public SteamService(string token)
        {
            steamUser = WebAPI.GetInterface("ISteamUser", token);
            steamUser.Timeout = TimeSpan.FromSeconds(5);
        }

        public string GetSteamName(string id)
        {
            string name = "";
            foreach (KeyValue user in steamUser.GetPlayerSummaries(steamids: id)["players"].Children)
            {
                name += user["personaname"].GetType();
            }
            return name;
        }

        /*Redundant
        public string getID(string url)
        {
            return steamUser.ResolveVanityURL(vanityurl: url)["steamid"].AsString();
        }*/
    }
}