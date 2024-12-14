using Cosmos.System.Graphics;
using System;
using System.Drawing;
using Waterfall.System.Graphics.WaterfallGraphics;

namespace Waterfall.System.Graphics.Components
{
    public static class BitmapEditor
    {
        /// <summary>
        /// Modifies the colors of the pixels in the bitmap, shifting them towards a target color with a specified intensity.
        /// </summary>
        /// <param name="bitmap">The bitmap to modify.</param>
        /// <param name="targetColor">The target color to shift the pixels towards.</param>
        /// <param name="intensity">The intensity of the color shift, ranging from 0.0f (no change) to 1.0f (full change).</param>
        /// <returns>A new bitmap with the modified colors.</returns>
        public static void ModifyColor(Bitmap bitmap, Color targetColor, float intensity)
        {
            if (intensity < 0.0f || intensity > 1.0f)
                return;

            int[] originalData = bitmap.RawData;

            int[] bmpData = new int[originalData.Length];
            Array.Copy(originalData, bmpData, originalData.Length);

            for (int i = 0; i < bmpData.Length; i++)
            {
                int color = bmpData[i];

                byte a = (byte)((color >> 24) & 0xFF);
                byte r = (byte)((color >> 16) & 0xFF);
                byte g = (byte)((color >> 8) & 0xFF);
                byte b = (byte)(color & 0xFF);

                int deltaR = targetColor.R - r;
                int deltaG = targetColor.G - g;
                int deltaB = targetColor.B - b;

                deltaR = (int)(deltaR * intensity);
                deltaG = (int)(deltaG * intensity);
                deltaB = (int)(deltaB * intensity);

                r = (byte)Math.Max(0, Math.Min(255, r + deltaR));
                g = (byte)Math.Max(0, Math.Min(255, g + deltaG));
                b = (byte)Math.Max(0, Math.Min(255, b + deltaB));

                bmpData[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }
            bitmap.RawData = bmpData;
        }
        public static Bitmap ReturnModifedColor(Bitmap bitmap, Color targetColor, float intensity)
        {
            if(bitmap == null) return null;
            if (intensity < 0.0f || intensity > 1.0f)
                return null;

            int[] originalData = bitmap.RawData;

            int[] bmpData = new int[originalData.Length];
            Array.Copy(originalData, bmpData, originalData.Length);

            for (int i = 0; i < bmpData.Length; i++)
            {
                int color = bmpData[i];

                byte a = (byte)((color >> 24) & 0xFF);
                byte r = (byte)((color >> 16) & 0xFF);
                byte g = (byte)((color >> 8) & 0xFF);
                byte b = (byte)(color & 0xFF);

                int deltaR = targetColor.R - r;
                int deltaG = targetColor.G - g;
                int deltaB = targetColor.B - b;

                deltaR = (int)(deltaR * intensity);
                deltaG = (int)(deltaG * intensity);
                deltaB = (int)(deltaB * intensity);

                r = (byte)Math.Max(0, Math.Min(255, r + deltaR));
                g = (byte)Math.Max(0, Math.Min(255, g + deltaG));
                b = (byte)Math.Max(0, Math.Min(255, b + deltaB));

                bmpData[i] = (a << 24) | (r << 16) | (g << 8) | b;
            }
            Bitmap bmp = new Bitmap(bitmap.Width, bitmap.Height, bitmap.Depth)
            {
                RawData = bmpData
            };
            return bmp;
        }

        public static Bitmap ResizeToScreen(this Bitmap bitmap)
        {
            if(bitmap == null) return null;
            int[] originalData = bitmap.RawData;
            int[] bmpData = new int[originalData.Length];
            Array.Copy(originalData, bmpData, originalData.Length);
            Bitmap bmp = new Bitmap(bitmap.Width, bitmap.Height, bitmap.Depth);
            bmp.RawData = bmpData;
            if (bmp.Width != GUI.ScreenWidth || bmp.Height != GUI.ScreenHeight)
            {
                bmp = bmp.Resize(GUI.ScreenWidth, GUI.ScreenHeight);
            }
            return bmp;
        }
    }
}
