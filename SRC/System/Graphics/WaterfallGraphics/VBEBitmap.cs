using Cosmos.System.Graphics;
using System.Drawing;

namespace Waterfall.System.Graphics.WaterfallGraphics
{
    public class VBEBitmap
    {
        public Color AlphaBlend(Color to, Color from, byte alpha)
        {
            float alphaFactor = alpha / 255f;

            byte R = (byte)((to.R * alphaFactor) + (from.R * (1 - alphaFactor)));
            byte G = (byte)((to.G * alphaFactor) + (from.G * (1 - alphaFactor)));
            byte B = (byte)((to.B * alphaFactor) + (from.B * (1 - alphaFactor)));
            byte finalAlpha = (byte)((to.A * alphaFactor) + (from.A * (1 - alphaFactor)));

            return Color.FromArgb(finalAlpha, R, G, B);
        }

        public Color GetPointColor(Bitmap image, int X, int Y)
        {
            return Color.FromArgb(image.RawData[X + (Y * image.Width)]);
        }
    }
}
