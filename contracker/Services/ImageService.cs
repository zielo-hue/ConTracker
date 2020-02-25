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
        
        public MemoryStream GetProfilePicture(CachedUser user)
        {
            using var client = new WebClient();
            return new MemoryStream(buffer: client.DownloadData(user.GetAvatarUrl()));
        }
        
        public void OverlayTest(CachedUser user, MemoryStream output)
        {
            // var player = ContrackerService.GetPlayer(discordId: user.Id.ToString());
            using var profilePictureStream = GetProfilePicture(user);
            profilePictureStream.Position = 0;

            using var profilePicture = Image.Load(profilePictureStream);
            using var background = Image.Load("Resources/Images/contracker.jpg");
            profilePicture.Mutate(x => x.Resize(new Size(100, 100)));
            background.Mutate(x => x
                .Resize(new Size(450, 300))
                .DrawImage(profilePicture, new Point(30, 30), 1f));
                
            background.SaveAsJpeg(output);
        }
    }
}