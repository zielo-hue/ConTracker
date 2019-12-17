using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace frobot
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            var conf = JObject.Parse(File.ReadAllText(@"config.json"));
            var token = (string) conf["DISCORD"]["TOKEN"];
            using var bot = new DiscordBot(TokenType.Bot, token, new DiscordBotConfiguration
            {
                Prefixes = new[] { "!!" },
                ProviderFactory = bot => new ServiceCollection()
                .AddSingleton((DiscordBot)bot)
                .AddSingleton(new Services.RedditService((DiscordBot) bot))
                .BuildServiceProvider()
            });
            bot.Logger.MessageLogged += this.Logger_MessageLogged;
            bot.AddModules(Assembly.GetExecutingAssembly());

            var guild = await bot.GetGuildAsync(535572626480037909);
            var channels = await guild.GetChannelsAsync();
            var channel = channels.OfType<RestTextChannel>().FirstOrDefault(x => x.Name == "general");

            bot.Run();
        }

        private void Logger_MessageLogged(object sender, Disqord.Logging.MessageLoggedEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}