using Disqord;
using Disqord.Bot;
using Qmmands;
using System;
using System.Threading.Tasks;
using contracker.Services;

namespace contracker.Modules
{
    public sealed class RedditModule : DiscordModuleBase
    {
        public RedditService RedditService { get; set; }
        static Random rnd = new Random();

        [Command("leddit")]
        public Task PingAsync()
            => ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle("Bruh")
                .WithColor(Color.Azure)
                .Build());

        /*[Command("top")]
        public async Task RedditAsync()
        {
            var topPost = tf2.Posts.Hot[rnd.Next(0, 15)];
            var selftext = topPost.Listing.IsSelf ? ((SelfPost) topPost).SelfText : "(linkpost)";
            await ReplyAsync(embed: new LocalEmbedBuilder()
                .WithTitle(topPost.Title)
                .WithUrl("https://www.reddit.com/r/tf2/comments/" + topPost.Id)
                .WithDescription(selftext)
                .AddField("Author", value: "u/" + topPost.Author, true)
                .AddField("Upvotes", value: topPost.UpVotes.ToString(), true)
                .AddField("Id: ", value: topPost.Id, true)
                .WithImageUrl(((LinkPost) topPost).URL)
                .WithColor(Color.Azure)
                .Build()).ConfigureAwait(true);
        }*/
    }
}
