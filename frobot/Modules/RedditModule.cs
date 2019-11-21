using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using frobot.Services;
using Qmmands;

namespace frobot.Modules
{
    public sealed class RedditModule : DiscordModuleBase
    {
        public RedditService redditService { get; set; }

        [Command("reddittest")]
        public Task PingAsync()
            => ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle("Bruh")
                .WithColor(Color.Azure)
                .Build());
    }
}
