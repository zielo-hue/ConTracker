using System;
using SteamKit2;

namespace contracker.Services
{
    public class SteamService
    {
        string _token;
        public SteamService(string token)
        {
            _token = token;
        }

        public string GetSteamName(string id)
        {
            string name = "";
            using ( dynamic steamUser = WebAPI.GetInterface("ISteamUser", _token) )
            {
                steamUser.Timeout = TimeSpan.FromSeconds(5);
                KeyValue response = steamUser.GetPlayerSummaries(steamids: id);
                KeyValue user = response["players"]["player"]["0"];
                name = user["personaname"].AsString();
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