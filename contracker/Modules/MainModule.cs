using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using contracker.Services;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Qmmands;
using SteamKit2;

namespace contracker.Modules
{
    public sealed class MainModule : DiscordModuleBase
    {
        public DiscordBotSharder bot { get; set; }
        public DUserService DUserService { get; set; }
        public SteamService SteamService { get; set; }

        [Command("help", "commands")]
        [Description("Lists available commands.")]
        public Task HelpAsync()
        => ReplyAsync(embed: new LocalEmbedBuilder()
            .WithTitle("Commands")
            .WithDescription(string.Join("\n", bot.GetAllCommands().Select(
                x => $"`{x.Name}` - {x.Description}")))
            .WithColor(Color.Honeydew)
            .Build());

        [Command("ping")]
        [Description("Placeholder.")]
        public Task PingAsync()
            => ReplyAsync("pong");

        [Command("id")]
        [Description("Placeholder.")]
        public async Task IdAsync(Snowflake id)
        {
            string description;
            if (DUserService.GetAccounts(id).Result.Count == 0)
                description = "`No Steam Accounts associated with this account.`";
            else
                description = string.Join("\n", DUserService.GetAccounts(id).Result.Select(x => $"`{x}`"));
            
            await ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle("Steam Account Ids")
                .WithDescription(description)
                .WithColor(Color.Honeydew)
                .Build()).ConfigureAwait(true);
        }

        [Command("contracker", "progress", "c")]
        [Description("Placeholder for checking contracker progress.")]
        public async Task TrackerAsync()
        {
            Snowflake id = Context.User.Id;
        }

        [Command("register")]
        [Description("Placeholder for registering.")]
        public async Task RegisterAsync(int accountNumber = -1)
        {
            Snowflake id = Context.User.Id;
            var users = DUserService.GetAccounts(id).Result;
            string title;
            string description;
            switch (users.Count)
            {
                case 0:
                    title = "Fail";
                    description = "`No Steam accounts associated with your account.`\n" +
                                  "[Go here](https://www.quora.com/How-will-I-add-my-gaming-accounts-in-Discord) " +
                                  "to see how to link your Steam account. You may unlink your account once you register.";
                    break;
                case 1:
                    if (DUserService.IsVerified(id).Result)
                    {
                        title = "Success";
                        description = "`Pretend this sends an API request..`";
                    }
                    else
                    {
                        title = "Fail";
                        description = "`Your Steam account is not verified.`\n" +
                                      "[Go here](https://www.reddit.com/r/discordapp/comments/6elfxl/its_now_possible_to_have_a_verified_steam_account/) " +
                                      "to see how to verify your Steam account.";
                    }
                    break;
                default:
                    if (accountNumber != -1)
                    {
                        title = "Success";
                        description = "`Pretend this sends an API request...`\nwith account " +
                                          $"`{SteamService.GetSteamName(users[accountNumber]).Replace("`", "")}` " +
                                          $"or steamid `{users[accountNumber]}`";
                    }
                    else
                    {
                        title = "Fail";
                        description = "`You have multiple linked Steam accounts.`\n" +
                                    "Use command `!register n` and replace `n` with the correct account number shown below." +
                                    "```\n" +
                                    string.Join("\n",
                                        users.Select((x, index) =>
                                            $"{index} - {SteamService.GetSteamName(x).Replace("`", "")}")) +
                                    "```";
                    }
                    break;
            }

            await ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(Color.Honeydew)
                .Build()).ConfigureAwait(true);
        }

    }
}