using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Qmmands;

namespace contracker.Services
{
    public class DUserService
    {
        private DiscordClient client;
        
        public DUserService(string token)
        {
            client = new DiscordClient(TokenType.User, token);
        }

        // Use steam API
    }
}