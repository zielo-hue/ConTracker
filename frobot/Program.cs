using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;

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
            var token = "NTk3OTkwMzA3NzExNDE4MzY5.XSQIBw.KoFi6HRl2gr08dxX5PRWLQRs3uc";
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

            var guild = await bot.GetGuildAsync(597874957237878814);
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