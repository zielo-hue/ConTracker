using System;
using System.IO;
using System.Threading.Tasks;
using contracker.Services;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Disqord.Bot.Prefixes;
using contracker.Modules;
using contracker.Resources;
using Disqord.Extensions.Interactivity;
using Disqord.Extensions.Interactivity.Menus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Qmmands;
using Qommon.Events;
using Color = System.Drawing.Color;

namespace contracker
{
    internal sealed class Program : DiscordBotSharder
    {
        static readonly JObject Conf = JObject.Parse(File.ReadAllText(@"config.json"));
        static readonly String BotToken = (string)Conf["DISCORD"]["TOKEN"];
        static readonly String ClientToken = (string)Conf["DISCORD_USER"]["TOKEN"];
        static readonly String SteamToken = (string)Conf["STEAM"]["TOKEN"];
        static readonly String ContrackerToken = (string)Conf["CONTRACKER_API"]["TOKEN"];
        private static void Main()
            => new Program().Run();

        private Program() : base(TokenType.Bot, BotToken,
            new DefaultPrefixProvider()
                .AddPrefix("c!")
                .AddMentionPrefix(),
            new DiscordBotConfiguration
            {
                Status = UserStatus.Online,
                ProviderFactory = bot =>
                    new ServiceCollection()
                        .AddSingleton((DiscordBotSharder)bot)
                        .AddSingleton<DBotService>()
                        .AddSingleton(new ImageService())
                        .AddSingleton(new DUserService(ClientToken))
                        .AddSingleton(new SteamService(SteamToken))
                        .AddSingleton(new ContrackerService(ContrackerToken))
                        .BuildServiceProvider(),
                CommandService = new CommandService(new CommandServiceConfiguration
                {
                    CooldownBucketKeyGenerator = (_, __) =>
                    {
                        // Casting
                        var type = (CooldownBucketType)_;
                        var context = (DiscordCommandContext)__;

                        var application = context.Bot.CurrentApplication.FetchAsync();
                        // Exclude bot owner
                        if (context.User.Id == application.Result.Owner.Id)
                            return null;

                        // Enums Etc.
                        switch (type)
                        {
                            case CooldownBucketType.User:
                                return context.User.Id;
                            case CooldownBucketType.Channel:
                                return context.Channel.Id;
                            case CooldownBucketType.Guild:
                                return context.Guild.Id;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(type));
                        }
                    }
                }),
            })
        {
            Logger.MessageLogged += MessageLogged;

            // In case of fuckup:
            CommandExecutionFailed += handler;

            AddModules(typeof(Program).Assembly);

            // Initialize services
            this.GetRequiredService<DUserService>();
            this.GetRequiredService<SteamService>();
            this.GetRequiredService<DBotService>();

            // Extensions
            AddExtensionAsync(new InteractivityExtension());
        }

        protected override ValueTask AfterExecutedAsync(IResult result, DiscordCommandContext context)
        {
            if (result is CommandOnCooldownResult cooldownResult)
            {
                var cooldownTime = cooldownResult.Cooldowns;
                context.Channel.SendMessageAsync(embed: new LocalEmbedBuilder()
                    .WithTitle($"You are on cooldown!")
                    .WithColor(Color.Red).Build());
            }

            return base.AfterExecutedAsync(result, context);
        }

        private void MessageLogged(object sender, Disqord.Logging.MessageLoggedEventArgs e)
            => Console.WriteLine(e);

        // In case of fuckup:
        private Task handler(CommandExecutionFailedEventArgs args)
        {
            Console.WriteLine(args.Result.Exception.ToString());
            throw args.Result.Exception;
        }

    }
}