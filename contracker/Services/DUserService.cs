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
        private readonly DiscordBotSharder _bot;
        
        public DUserService(DiscordBotSharder bot, string token)
        {
            _bot = bot;
            client = new DiscordClient(TokenType.User, token);
            _ = UpdatePresence();
        }
        
        // i forgot to do this
        async Task UpdatePresence()
        {
            while (true)
            {
                await Task.Delay(30000);
                await _bot.SetPresenceAsync(new LocalActivity("FUCK holonoid & gaming", ActivityType.Playing));
            }
        }

        // Use steam API
    }
}