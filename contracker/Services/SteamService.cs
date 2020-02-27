using System;
using System.Net.Sockets;
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
                try
                {
                    KeyValue response = steamUser.GetPlayerSummaries(steamids: id);
                    KeyValue user = response["players"]["player"]["0"];
                    name = user["personaname"].AsString();
                }
                catch (WebAPIRequestException e)
                {
                    name = "STEAM API IS DOWN, PRETEND YOUR NAME IS HERE";
                    Console.WriteLine("steam api do the " + e);
                }
            }
            return name;
        }
    }
}