using System;
using System.IO;
using System.Threading.Tasks;
using contracker.Services;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Disqord.Bot.Prefixes;
using contracker.Modules;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Qmmands;
using Qommon.Events;

namespace contracker
{
    internal sealed class Program : DiscordBotSharder
    {
        static JObject conf = JObject.Parse(File.ReadAllText(@"config.json"));
        static String botToken = (string) conf["DISCORD"]["TOKEN"];
        static String clientToken = (string) conf["DISCORD_USER"]["TOKEN"];
        private static String steamToken = (string) conf["STEAM"]["TOKEN"];
        private static void Main()
            => new Program().Run();
        
        private Program() : base(TokenType.Bot, botToken,
            new DefaultPrefixProvider()
                .AddPrefix("!c")
                .AddMentionPrefix(),
            new DiscordBotConfiguration
            {
                Status = UserStatus.Online,
                ProviderFactory = bot => new ServiceCollection()
                    .AddSingleton((DiscordBotSharder) bot)
                    .AddSingleton(new DUserService(clientToken))
                    .AddSingleton(new SteamService(steamToken))
                    .BuildServiceProvider()
            })
        {
            Logger.MessageLogged += MessageLogged;
            
            /*In case of fuckup:
            CommandExecutionFailed += handler;*/
            
            AddModules(typeof(Program).Assembly);
            
            // Initialize services
            this.GetRequiredService<DUserService>();
            this.GetRequiredService<SteamService>();
        }

        private void MessageLogged(object sender, Disqord.Logging.MessageLoggedEventArgs e)
            => Console.WriteLine(e);
        
         /*
         // In case of fuckup:
         private async Task handler(CommandExecutionFailedEventArgs args)
           => Console.WriteLine(args.Result.Exception);
        */
         
    }
}