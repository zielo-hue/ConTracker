using System;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new MemoryStream();
            using (var template = Image.Load("whatthefuck.jpg"))
            {
                Font font = SystemFonts.CreateFont("Arial", 10);

                using (var img = template.Clone(ctx => ctx.ApplyScalingWaterMark()))
                {
                    img.Save("bruh.jpg");
                }
            }
        }

        private static IImageProcessingContext ApplyScalingWaterMark(this IImageProcessingContext processingContext,
            Font font,
            string text,
            Color color,
            float padding,
            bool wordwrap)
        {
            if (wordwrap)
                return processingContext.ApplyScalingWaterMarkWordWrap(font, text, color, padding);
            else
                return processingContext.ApplyScalingWaterMarkSimple(font, text, color, padding);
        }
        
        
    }
}