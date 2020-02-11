using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using contracker.Resources;
using contracker.Services;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Disqord.Extensions.Interactivity.Menus;
using ImageSandbox;
using Qmmands;
using SteamKit2;

namespace contracker.Modules
{
    public sealed class MainModule : DiscordModuleBase
    {
        public DiscordBotSharder bot { get; set; }
        public DUserService DUserService { get; set; }
        public SteamService SteamService { get; set; }
        public ContrackerService ContrackerService { get; set; }
        public ImageService ImageService { get; set; }

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
            var id = Context.User.Id;
            var description = $"You are not registered! `!cregister` to register.";
            var builder = new LocalEmbedBuilder()
                .WithTitle("Tracker template");
            if (ContrackerService.IsRegistered(id))
            {
                var player = ContrackerService.Contracker.GetPlayer(discordId: id.ToString());
                var contracts = ContrackerService.Contracker.GetPlayerContracts(player);
                if (contracts.Count > 0)
                    description = string.Join("\n",
                        contracts.Select(x =>
                            $"`{x.Contract.Name}` - {x.Contract.Primary.First().Description}\n" +
                            $"\tPoints: {x.Contract.Primary.First().Points}"));
                else
                    description = "`You have no active contracts!`";
            }
            await ReplyAsync(embed: new LocalEmbedBuilder()
            .WithDescription(description)
            .WithColor(Color.Honeydew)
            .Build()).ConfigureAwait(true);
        }
        
        [Command("profile", "me")]
        [Description("Placeholder for profile.")]
        public async Task ProfileAsync()
        {
            var id = Context.User.Id;
            var color = Color.Red;
            var description = "You are not registered!";
            var embed = new LocalEmbedBuilder()
                .WithTitle("User profile");
            if (ContrackerService.IsRegistered(id))
            {
                var player = ContrackerService.Contracker
                    .GetPlayer(discordId: id.ToString());
                description = "You are gamer.";
                color = Color.Green;
                embed.AddField("Discord ID", player.Discord)
                    .AddField("Steam ID", player.Steam)
                    .AddField("Steam Name", SteamService.GetSteamName(player.Steam))
                    .AddField("Point", player.Points)
                    .AddField("Stars", player.Stars);
            }

            await ReplyAsync(embed: embed
                .WithDescription(description)
                .WithColor(color)
                .Build()).ConfigureAwait(true);
        }

        [Command("register")]
        [Description("Placeholder for registering.")]
        [Cooldown(4, 5, CooldownMeasure.Seconds, CooldownBucketType.User)]
        public async Task RegisterAsync(int accountNumber = -1)
        {
            Snowflake id = Context.User.Id;
            var users = DUserService.GetAccounts(id).Result;
            string title;
            string description;
            if (!ContrackerService.IsRegistered(id))
            {
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
                            try
                            {
                                title = "Success";
                                description = "`API request sent...`";
                                ContrackerService.Contracker.CreatePlayer(users.First(), id.ToString());
                            }
                            catch (XmlException e)
                            {
                                title = "Fail";
                                description = "`An error has occured. Probably server's fault.`";
                                Console.WriteLine(e);
                            }
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
                        if (accountNumber != -1 && accountNumber < users.Count)
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
            }
            else
            {
                var player = ContrackerService.Contracker.GetPlayer(discordId: id.ToString());
                title = "You are already registered!";
                description = "You are registered on Steam account " +
                              $"`{SteamService.GetSteamName(player.Steam)}`";
            }
            await ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(Color.Honeydew)
                .Build()).ConfigureAwait(true);
        }

        [Command("vote")]
        [Description("Reaction based voting test")]
        public async Task ReactionMenuAsync()
        {
            var menu = new RegistrationMenu();
            await Context.Channel.StartMenuAsync(menu);
        }

        [Command("bruh")]
        [Description("Image generation test")]
        public async Task ImageAsync(string bruh)
        {
            using (var image = new MemoryStream())
            {
                CaptionGenerator.SimpleCaption(bruh, image);
                image.Position = 0;
                await ReplyAsync(new LocalAttachment(image, "gay.jpg")).ConfigureAwait(true);
            }
        }

        [Command("brug")]
        [Description("Image generation test, but epic")]
        public async Task ImageTestAsync()
        {
            using (var image = new MemoryStream())
            {
                ImageService.OverlayTest(Context.User, image);
                image.Position = 0;
                await ReplyAsync(new LocalAttachment(image, "brug.jpg")).ConfigureAwait(true);
            }
        }
    }
}