using System;
using System.IO;
using System.Threading.Tasks;
using contracker.Services;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Disqord.Bot.Prefixes;
using contracker.Modules;
using Disqord.Extensions.Interactivity;
using Disqord.Extensions.Interactivity.Menus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Qmmands;
using Qommon.Events;

namespace contracker
{
    internal sealed class Program : DiscordBotSharder
    {
        static readonly JObject Conf = JObject.Parse(File.ReadAllText(@"config.json"));
        static readonly String BotToken = (string) Conf["DISCORD"]["TOKEN"];
        static readonly String ClientToken = (string) Conf["DISCORD_USER"]["TOKEN"];
        static readonly String SteamToken = (string) Conf["STEAM"]["TOKEN"];
        static readonly String ContrackerToken = (string) Conf["CONTRACKER_API"]["TOKEN"];
        private static void Main()
            => new Program().Run();
        
        private Program() : base(TokenType.Bot, BotToken,
            new DefaultPrefixProvider()
                .AddPrefix("!c")
                .AddMentionPrefix(),
            new DiscordBotConfiguration
            {
                Status = UserStatus.Online,
                ProviderFactory = bot =>
                    new ServiceCollection()
                        .AddSingleton((DiscordBotSharder) bot)
                        .AddSingleton<DBotService>()
                        .AddSingleton(new DUserService(ClientToken))
                        .AddSingleton(new SteamService(SteamToken))
                        .AddSingleton(new ContrackerService(ContrackerToken))
                        .BuildServiceProvider()
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
        }

        private void MessageLogged(object sender, Disqord.Logging.MessageLoggedEventArgs e)
            => Console.WriteLine(e);
        
         // In case of fuckup:
         private async Task handler(CommandExecutionFailedEventArgs args)
           => Console.WriteLine(args.Result.Exception);
         
    }
}