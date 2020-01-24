using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2;
using System.Net.Http;

namespace contracker.Services
{
    public class SteamService
    {
        public SteamService(string token)
        {
            dynamic steamNews = WebAPI.GetInterface("ISteamUser");
        }
    }
}