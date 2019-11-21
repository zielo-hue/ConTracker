using Disqord;
using Disqord.Bot;
using Reddit;
using System;
using System.Timers;

namespace frobot.Services
{
    class RedditService
    {
        RedditAPI r = new RedditAPI("zyUACyX76CQ1Lw", "322307119885-Vdx9Piiy89_iMYreR0PxA0Ro0E4");
        private readonly DiscordBot _bot;
        private readonly Timer _timer;
        private readonly Snowflake channelid = 597875185886035989;
        public RedditService(DiscordBot bot)
        {
            _bot = bot;
            _timer = new Timer();
            _timer.Interval = 30000;
            _timer.Elapsed += UpdateStatus;
        }

        private async void UpdateStatus(Object source, System.Timers.ElapsedEventArgs e)
        {
            await _bot.SetPresenceAsync(new LocalActivity("bruh", ActivityType.Playing));
        }
    }
}
