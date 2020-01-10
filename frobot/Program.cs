using System;
using System.IO;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Disqord.Bot.Prefixes;
using frobot.Modules;
using frobot.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Qmmands;
using Qommon.Events;

namespace frobot
{
    internal sealed class Program : DiscordBotSharder
    {
        static JObject conf = JObject.Parse(File.ReadAllText(@"config2.json"));
        static String token = (string) conf["DISCORD"]["TOKEN"];
        private static void Main()
            => new Program().Run();
        
        private Program() : base(TokenType.Bot, token,
            new DefaultPrefixProvider()
                .AddPrefix("!!")
                .AddMentionPrefix(),
            new DiscordBotConfiguration
            {
                Status = UserStatus.Online,
                ProviderFactory = bot => new ServiceCollection()
                    .AddSingleton((DiscordBotSharder) bot)
                    .AddSingleton<RedditService>()
                    .AddSingleton<SteamService>()
                    .BuildServiceProvider()
            })
        {
            Logger.MessageLogged += MessageLogged;
            
            // In case of fuckup:
            // CommandExecutionFailed += handler;
            
            AddModules(typeof(Program).Assembly);
            
            // Initialize services
            this.GetRequiredService<RedditService>();
            this.GetRequiredService<SteamService>();
        }

        private void MessageLogged(object sender, Disqord.Logging.MessageLoggedEventArgs e)
            => Console.WriteLine(e);
        
        /* In case of fuckup:
         * private async Task handler(CommandExecutionFailedEventArgs args)
         *   => Console.WriteLine(args.Result.Exception);
         */
    }
}