using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Qmmands;

namespace contracker.Modules
{
    public sealed class MainModule : DiscordModuleBase
    {
        public DiscordBotSharder bot { get; set; }

        [Command("help", "commands"), Cooldown(1, 5, CooldownMeasure.Seconds, )]
        [Description("Lists available commands.")]
        public Task HelpAsync()
        => ReplyAsync(embed: new LocalEmbedBuilder(
            .WithTitle("Commands")
            .WithDescription(string.Join("\n", bot.GetAllCommands().Select(
                x => $"`{x.Name}` - {x.Description}")))
            .WithColor(Color.Honeydew)
            .Build());

        [Command("ping"), Cooldown(1, 5, CooldownMeasure.Seconds, )]
        public Task PingAsync()
            => ReplyAsync("pong");
    }
}