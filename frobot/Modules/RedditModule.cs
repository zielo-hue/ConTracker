using Disqord;
using Disqord.Bot;
using frobot.Services;
using Qmmands;
using Reddit.Controllers;
using System;
using System.Threading.Tasks;

namespace frobot.Modules
{
    public sealed class RedditModule : DiscordModuleBase
    {
        public RedditService RedditService { get; set; }
        Random rnd = new Random();

        Subreddit tf2 = RedditService.r.Subreddit("tf2").About();

        [Command("reddittest")]
        public Task PingAsync()
            => ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle("Bruh")
                .WithColor(Color.Azure)
                .Build());

        [Command("rpost")] //TODO: Implement
        public Task RedditPostAsync()
            => ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle("Ok")
                .WithColor(Color.Azure)
                .Build());

        [Command("rtf2top")]
        public async Task RedditAsync()
        {
            var topPost = tf2.Posts.Hot[rnd.Next(0, 10)];
            var selftext = topPost.Listing.IsSelf ? ((SelfPost)topPost).SelfText : "(linkpost)";
            await ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle(topPost.Title)
                .WithUrl("https://www.reddit.com/r/tf2/comments/" + topPost.Id)
                .WithDescription(selftext)
                .AddField("Author", value: "u/" + topPost.Author, true)
                .AddField("Upvotes", value: topPost.UpVotes.ToString(), true)
                .AddField("Id: ", value: topPost.Id, true)
                .WithImageUrl(((LinkPost)topPost).URL)
                .WithColor(Color.Azure)
                .Build()).ConfigureAwait(true);
        }
    }
}
