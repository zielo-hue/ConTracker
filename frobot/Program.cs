using System;
using System.IO;
using Disqord;
using Disqord.Bot;
using Disqord.Bot.Sharding;
using Disqord.Bot.Prefixes;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

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
                .AddPrefix("~~")
                .AddMentionPrefix(),
            new DiscordBotConfiguration
            {
                Status = UserStatus.Invisible,
                ProviderFactory = bot => new ServiceCollection()
                    .AddSingleton((DiscordBot) bot)
                    .AddSingleton(new Services.RedditService((DiscordBot) bot))
                    .BuildServiceProvider()
            })
        {
            Logger.MessageLogged += MessageLogged;
            AddModules(typeof(Program).Assembly);
        }

        private void MessageLogged(object sender, Disqord.Logging.MessageLoggedEventArgs e)
            => Console.WriteLine(e);
    }
}