using Disqord.Bot.Sharding;

namespace contracker.Services
{
    public class DBotService
    {
        public DiscordBotSharder _bot;

        // Temporary solution
        public DBotService(DiscordBotSharder bot)
        {
            _bot = bot;
        }
    }
}