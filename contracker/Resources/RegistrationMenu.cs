using System.Threading.Tasks;
using Disqord;
using Disqord.Extensions.Interactivity.Menus;

namespace contracker.Resources
{
    public sealed class RegistrationMenu : MenuBase
    {
        protected override async Task<IUserMessage> InitialiseAsync()
        {
            var message = await Channel.SendMessageAsync("vote menu thing!");
            return message;
        }

        [Button("<:trans_heel:590794948262101003>")]
        public Task FirstOptionAsync(ButtonEventArgs e)
        {
            var content = e.WasAdded ? "trans heel" : "regular heel";
            return Message.ModifyAsync(x => x.Content = content);
        }
        
        [Button("👠")]
        public Task SecondOptionAsync(ButtonEventArgs e)
        {
            var content = e.WasAdded ? "regular heel" : "trans heel";
            return Message.ModifyAsync(x => x.Content = content);
        }
    }
}