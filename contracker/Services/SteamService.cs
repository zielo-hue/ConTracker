using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;
using System.Net.Http;
using Disqord;

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

        /*Redundant
        public string getID(string url)
        {
            return steamUser.ResolveVanityURL(vanityurl: url)["steamid"].AsString();
        }*/
    }
}