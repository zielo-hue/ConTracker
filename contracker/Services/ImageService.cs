using System.IO;
using System.Net;
using contrackerNET;
using Disqord;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace contracker.Services
{
    public class ImageService
    {
        public ContrackerService ContrackerService { get; set; }
        public void GetProfilePicture(CachedUser user, MemoryStream output)
        {
            using ( var profilePictureStream = new MemoryStream(new WebClient().DownloadData(user.GetAvatarUrl())))
            {
                output = profilePictureStream;
            }
        }
        public void OverlayTest(CachedUser user, MemoryStream output)
        {
            var player = ContrackerService.GetPlayer(discordId: user.Id.ToString());
            var profilePictureStream = new MemoryStream();
            profilePictureStream.Position = 0;
            GetProfilePicture(user, profilePictureStream);
            profilePictureStream.Position = 0;

            using var profilePicture = Image.Load(profilePictureStream);
            using var background = Image.Load("contracker.jpg");
            profilePicture.Mutate(x => x.Resize(new Size(100, 100)));
            background.Mutate(x => x
                .Resize(new Size(450, 300))
                .DrawImage(profilePicture, new Point(30, 30), 1f));
                
            background.SaveAsJpeg(output);
        }
    }
}