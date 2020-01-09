using System.Threading.Tasks;
using Disqord.Bot;
using Qmmands;

namespace frobot.Modules
{
    public sealed class DummyModule : DiscordModuleBase
    {
        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync("pong");
    }
}