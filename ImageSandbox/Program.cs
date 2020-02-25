using System;
using System.IO;
using System.Net;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace ImageSandbox
{
    static class Program
    {

        private const string longtx =
            "sedsfafdsgregesfwgqgfsdfwfqfdsaeqfqweffwfwqfwfgfqwesedsfafdsgregesfwgqgfsdfwfqfdsaeqfqweffwfwqfwfgfqwelj;dsfaojufqfdsfdaljzvcujpigdsfujvfda";
        static void Main(string[] args)
        {
            var output = new MemoryStream();
            var lol = Image.Load(new MemoryStream(buffer: new WebClient().DownloadData("https://cdn.discordapp.com/emojis/598551803566489611.png")));
            lol.SaveAsPng(output);
            /*using (var template = Image.Load("whatthefuck.jpg"))
            {
                Font font = SystemFonts.CreateFont("Arial", 10);

                using (var img = template.Clone(ctx =>
                    ctx.ApplyScalingWaterMark(font, "lol", Color.HotPink, 5, false)))
                {
                    img.Save("bruh.jpg");
                }

                using (var img = template.Clone(ctx =>
                    ctx.ApplyScalingWaterMark(font, longtx, Color.HotPink, 5, true)))
                {
                    img.Save("loe.jpg");
                }
            }*/
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

        private static IImageProcessingContext ApplyScalingWaterMarkSimple(this IImageProcessingContext processingContext,
            Font font,
            string text,
            Color color,
            float padding)
        {
            Size imgsize = processingContext.GetCurrentSize();
            float targetWidth = imgsize.Width - (padding * 2);
            float targetHeight = imgsize.Height - (padding * 2);
            
            SizeF size = TextMeasurer.Measure(text, new RendererOptions(font));
            float scalingFactor = Math.Min(imgsize.Width / size.Width, imgsize.Height / size.Height);
            
            Font scaledFont = new Font(font, scalingFactor * font.Size);
            
            var center = new PointF(imgsize.Width / 2, imgsize.Height / 2);
            var textGraphicOptions = new TextGraphicsOptions(true)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            return processingContext.DrawText(textGraphicOptions, text, scaledFont, color, center);
        }

        private static IImageProcessingContext ApplyScalingWaterMarkWordWrap(
            this IImageProcessingContext processingContext,
            Font font,
            string text,
            Color color,
            float padding)
        {
            Size imgSize = processingContext.GetCurrentSize();
            float targetWidth = imgSize.Width - (padding * 2);
            float targetHeight = imgSize.Height - (padding * 2);

            float targetMinHeight = imgSize.Height - (padding * 3);

            var scaledFont = font;
            SizeF s = new SizeF(float.MaxValue, float.MaxValue);

            float scaleFactor = (scaledFont.Size) / 2;
            int trapCount = (int) scaledFont.Size * 2;
            if (trapCount > 10)
                trapCount = 10;

            bool isTooSmall = false;

            while ((s.Height > targetHeight || s.Height < targetMinHeight) && trapCount > 0)
            {
                if (s.Height > targetHeight)
                {
                    if (isTooSmall)
                        scaleFactor = scaleFactor / 2;
                    scaledFont = new Font(scaledFont, scaledFont.Size - scaleFactor);
                    isTooSmall = false;
                }

                if (s.Height < targetHeight)
                {
                    if (!isTooSmall)
                        scaleFactor = scaleFactor / 2;
                    scaledFont = new Font(scaledFont, scaledFont.Size + scaleFactor);
                    isTooSmall = true;
                }

                trapCount--;

                s = TextMeasurer.Measure(text, new RendererOptions(scaledFont)
                {
                    WrappingWidth = targetWidth
                });
            }
            var center = new PointF(padding, imgSize.Height / 2);
            var textGraphicOptions = new TextGraphicsOptions(true)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                WrapTextWidth = targetWidth
            };
            return processingContext.DrawText(textGraphicOptions, text, scaledFont, color, center);
        }
    }
}