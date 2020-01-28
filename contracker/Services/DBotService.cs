﻿using System.Threading.Tasks;
using Disqord;
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
            _ = UpdatePresence();
        }
        
        async Task UpdatePresence()
        {
            while (true)
            {
                await Task.Delay(30000);
                await _bot.SetPresenceAsync(new LocalActivity("FUCK holonoid & gaming", ActivityType.Playing)).ConfigureAwait(true);
            }
        }
    }
}