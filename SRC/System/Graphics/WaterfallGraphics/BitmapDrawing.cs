using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;
using System.IO;

namespace Waterfall.System.Graphics.WaterfallGraphics
{
    public static class BitmapDrawing
    {

        static string ASC16Base64 = "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AAB4zMzM2MzGxsbMAAAAAAAA/sbGwMDAwMDAwAAAAAAAAAAA/mxsbGxsbGwAAAAAAAAA/sZgMBgwYMb+AAAAAAAAAAAAftjY2NjYcAAAAAAAAAAAZmZmZmZ8YGDAAAAAAAAAAHbcGBgYGBgYAAAAAAAAAH4YPGZmZjwYfgAAAAAAAAA4bMbG/sbGbDgAAAAAAAA4bMbGxmxsbGzuAAAAAAAAHjAYDD5mZmZmPAAAAAAAAAAAAH7b29t+AAAAAAAAAAAAAwZ+29vzfmDAAAAAAAAAHDBgYHxgYGAwHAAAAAAAAAB8xsbGxsbGxsYAAAAAAAAAAP4AAP4AAP4AAAAAAAAAAAAYGH4YGAAA/wAAAAAAAAAwGAwGDBgwAH4AAAAAAAAADBgwYDAYDAB+AAAAAAAADhsbGBgYGBgYGBgYGBgYGBgYGBgYGNjY2HAAAAAAAAAAABgYAH4AGBgAAAAAAAAAAAAAdtwAdtwAAAAAAAAAOGxsOAAAAAAAAAAAAAAAAAAAAAAAABgYAAAAAAAAAAAAAAAAAAAAGAAAAAAAAAAADwwMDAwM7GxsPBwAAAAAANhsbGxsbAAAAAAAAAAAAABw2DBgyPgAAAAAAAAAAAAAAAAAfHx8fHx8fAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
        static MemoryStream ASC16FontMS = new MemoryStream(Resources.zap_ext_light16);
        public static VBEBitmap VBE = new VBEBitmap();
        public static void DrawString(this Bitmap bmp, string str, PCScreenFont font, Color color, int x, int y)
        {
            int length = str.Length;
            byte width = font.Width;
            for (int i = 0; i < length; i++)
            {
                DrawChar(bmp, str[i], font, color, x, y);
                x += width;
            }
        }

        public static void DrawChar(this Bitmap bmp, char c, PCScreenFont font, Color color, int x, int y)
        {
            byte height = font.Height;
            byte width = font.Width;
            byte[] data = font.Data;
            int num = height * (byte)c;
            for (int i = 0; i < height; i++)
            {
                for (byte b = 0; b < width; b++)
                {
                    if (font.ConvertByteToBitAddress(data[num + i], b + 1))
                    {
                        bmp.SetPixel(color, (ushort)(x + b), (ushort)(y + i));
                    }
                }
            }
        }
        public static void DrawArray(this Bitmap bmp, int[] colors, int x, int y, int height)
        {
            for (int i = 0; i < height; i++)
            {
               colors.CopyTo(bmp.RawData, y*bmp.Width + x);
            }
        }

        public static void SetPixel(this Bitmap bmp, Color color, int X, int Y)
        {
            bmp.RawData[X + (Y * bmp.Width)] = color.ToArgb();
        }
        public static Color GetPixel(this Bitmap bmp, int X, int Y)
        {
            return Color.FromArgb(bmp.RawData[X + (Y * bmp.Width)]);
        }

        public static void SetRawPixel(this Bitmap bmp, int R, int G, int B, int X, int Y)
        {
            bmp.RawData[X + (Y * bmp.Width)] = (255 << 24) | (R << 16) | (G << 8) | B;
        }
        public static void SetRawPixel(this Bitmap bmp, int ARGB, int X, int Y)
        {
            bmp.RawData[X + (Y * bmp.Width)] = ARGB;
        }
        public static void SetRawPixel(this Bitmap bmp, int R, int G, int B, int A, int X, int Y)
        {
            bmp.RawData[X + (Y * bmp.Width)] = (A << 24) | (R << 16) | (G << 8) | B;
        }
        public static Bitmap FromBMPRegion(Bitmap canvas, int X, int Y, ushort W, ushort H)
        {
            Bitmap bitmap = new Bitmap(W, H, canvas.Depth);
            for (int i = X; i < W + X; i++)
            {
                for (int j = Y; j < H + Y; j++)
                {
                    bitmap.SetPixel(VBE.GetPointColor(canvas, i, j), i - X, j - Y);
                }
            }

            return bitmap;
        }
        public static void DrawPixel(this Bitmap bmp, int X, int Y, Color color)
        {
            if (color.A < 255)
            {
                color = VBE.AlphaBlend(color, Color.FromArgb(bmp.RawData[X + (Y * bmp.Width)]), color.A);
            }
            bmp.RawData[X + (Y * bmp.Width)] = color.ToArgb();

        }
        public static void DrawFilledRectangle(this Bitmap bmp, int X, int Y, int Width, int Height, Color color)
        {
            int col = color.ToArgb();
            int imageWidth = (int)bmp.Width;
            for (int y = 0; y < Height; y++)
            {
                int startIndex = X + ((Y + y) * imageWidth);

                Array.Fill(bmp.RawData, col, startIndex, Width);
            }
        }
        public static void Clear(this Bitmap bmp, Color col)
        {
            Array.Fill(bmp.RawData, col.ToArgb());
        }
        public static void DrawFilledCircle(this Bitmap bmp, Color color, int xCenter, int yCenter, int yR, int xR)
        {
            int bmpWidth = (int)bmp.Width;
            int bmpHeight = (int)bmp.Height;
            for (int i = -yR; i <= yR; i++)
            {
                for (int j = -xR; j <= xR; j++)
                {
                    if ((j * j * yR * yR) + (i * i * xR * xR) <= yR * yR * xR * xR)
                    {
                        // Sprawdzenie granic bitmapy
                        int x = xCenter + j;
                        int y = yCenter + i;

                        if (x >= 0 && x < bmpWidth && y >= 0 && y < bmpHeight)
                        {
                            bmp.DrawPixel(x, y, color);
                        }
                    }
                }
            }
        }

        public static void DrawArc(this Bitmap bmp, Color color, int x, int y, int width, int height, int startAngle = 0, int endAngle = 360)
        {
            if (width != 0 && height != 0)
            {
                for (double num = startAngle; num < endAngle; num += 0.5)
                {
                    double num2 = Math.PI * num / 180.0;
                    int num3 = (int)(width * Math.Cos(num2));
                    int num4 = (int)(height * Math.Sin(num2));
                    bmp.DrawPixel(x + num3, y + num4, color);
                }
            }
        }

        public static void DrawRectangle(this Bitmap bmp, int x, int y, int width, int height, Color color)
        {
            bmp.DrawLine(color, x, y, x + width, y);
            bmp.DrawLine(color, x + width, y, x + width, y + height);
            bmp.DrawLine(color, x, y + height, x + width, y + height);
            bmp.DrawLine(color, x, y, x, y + height);
        }


        public static void DrawLine(this Bitmap bmp, Color color, int x1, int y1, int x2, int y2)
        {

            // Bresenham's line algorithm
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {

                bmp.DrawPixel(x1, y1, color);

                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        public static void DrawImageAlpha(this Bitmap bmp, Bitmap image, int x, int y)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = Color.FromArgb(image.RawData[i + (j * image.Width)]);
                    if (color.A > 0)
                    {
                        if (color.A < 255)
                        {
                            color = VBE.AlphaBlend(color, Color.FromArgb(bmp.RawData[x + i + ((y + j) * bmp.Width)]), color.A);
                        }
                        bmp.RawData[x + i + ((y + j) * bmp.Width)] = color.ToArgb();
                    }

                }
            }
        }
        public static void DrawImage(this Bitmap bmp, Bitmap image, int x, int y)
        {
            for (int i = 0; i < image.Height; i++)
            {
                int bmpStartIndex = x + ((y + i) * (int)bmp.Width);
                int imageStartIndex = 0 + (i * (int)image.Width);

                Array.Copy(image.RawData, imageStartIndex, bmp.RawData, bmpStartIndex, image.Width);
            }
        }
        public static Bitmap GetImage(this Bitmap bmp, int x, int y, int width, int height)
        {
            Bitmap extractedImage = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
            for (int i = 0; i < height; i++)
            {
                int bmpStartIndex = x + ((y + i) * (int)bmp.Width);
                int imageStartIndex = 0 + (i * (int)extractedImage.Width);
                Array.Copy(bmp.RawData, bmpStartIndex, extractedImage.RawData, imageStartIndex, width);
            }
            return extractedImage;
        }

        public static Bitmap Blend(this Bitmap background, Bitmap image, byte alpha, bool blendImageAlpha)
        {
            // Create a new bitmap for the blended result with the size of the background
            Bitmap blendedBitmap = new Bitmap(background.Width, background.Height, ColorDepth.ColorDepth32);

            int[] blendedData = new int[background.Width * background.Height];

            byte alphaWeight = alpha;
            byte inverseAlphaWeight = (byte)(255 - alpha);

            int bgWidth = (int)background.Width;
            int bgHeight = (int)background.Height;
            int imgWidth = (int)image.Width;
            int imgHeight = (int)image.Height;

            // Precompute alpha ratios using bit shifts
            float alphaWeightRatio = alphaWeight / 255f;
            float inverseAlphaWeightRatio = inverseAlphaWeight / 255f;

            for (int y = 0; y < bgHeight; y++)
            {
                for (int x = 0; x < bgWidth; x++)
                {
                    int bgIndex = (y * bgWidth) + x;
                    int imgIndex = (y < imgHeight && x < imgWidth) ? (y * imgWidth) + x : -1;

                    int bgColor = background.RawData[bgIndex];
                    int imgColor = imgIndex >= 0 ? image.RawData[imgIndex] : bgColor;

                    byte bgAlpha = (byte)((bgColor >> 24) & 0xFF);
                    byte bgRed = (byte)((bgColor >> 16) & 0xFF);
                    byte bgGreen = (byte)((bgColor >> 8) & 0xFF);
                    byte bgBlue = (byte)(bgColor & 0xFF);

                    byte imgAlpha = (byte)((imgColor >> 24) & 0xFF);
                    byte imgRed = (byte)((imgColor >> 16) & 0xFF);
                    byte imgGreen = (byte)((imgColor >> 8) & 0xFF);
                    byte imgBlue = (byte)(imgColor & 0xFF);

                    byte resultAlpha;
                    byte resultRed;
                    byte resultGreen;
                    byte resultBlue;

                    if (blendImageAlpha)
                    {
                        if (imgAlpha == 0)
                        {
                            resultAlpha = bgAlpha;
                            resultRed = bgRed;
                            resultGreen = bgGreen;
                            resultBlue = bgBlue;
                        }
                        else
                        {
                            // Precompute normalized alphas
                            float imgAlphaNormalized = imgAlpha / 255f;
                            float bgAlphaNormalized = bgAlpha / 255f;

                            // Precompute the blending factors
                            float imgAlphaWeight = alphaWeightRatio * imgAlphaNormalized;
                            float bgAlphaWeight = inverseAlphaWeightRatio * bgAlphaNormalized;

                            resultAlpha = (byte)(Math.Min(1.0f, imgAlphaWeight + bgAlphaWeight) * 255);
                            resultRed = (byte)((((imgAlphaNormalized * imgRed) + ((1 - imgAlphaNormalized) * bgRed)) * alphaWeightRatio) + (bgRed * inverseAlphaWeightRatio));
                            resultGreen = (byte)((((imgAlphaNormalized * imgGreen) + ((1 - imgAlphaNormalized) * bgGreen)) * alphaWeightRatio) + (bgGreen * inverseAlphaWeightRatio));
                            resultBlue = (byte)((((imgAlphaNormalized * imgBlue) + ((1 - imgAlphaNormalized) * bgBlue)) * alphaWeightRatio) + (bgBlue * inverseAlphaWeightRatio));
                        }
                    }
                    else
                    {
                        resultAlpha = (byte)(((alphaWeight << 8) + (inverseAlphaWeight * bgAlpha)) >> 8);
                        resultRed = (byte)(((alphaWeight * imgRed) + (inverseAlphaWeight * bgRed)) >> 8);
                        resultGreen = (byte)(((alphaWeight * imgGreen) + (inverseAlphaWeight * bgGreen)) >> 8);
                        resultBlue = (byte)(((alphaWeight * imgBlue) + (inverseAlphaWeight * bgBlue)) >> 8);
                    }

                    blendedData[bgIndex] = (resultAlpha << 24) | (resultRed << 16) | (resultGreen << 8) | resultBlue;
                }
            }

            blendedBitmap.RawData = blendedData;

            return blendedBitmap;
        }

        public static Bitmap Blend(this Bitmap background, int[] rawData, byte alpha, bool blendImageAlpha)
        {
            // Create a new bitmap for the blended result with the size of the background
            Bitmap blendedBitmap = new Bitmap(background.Width, background.Height, ColorDepth.ColorDepth32);

            int[] blendedData = new int[background.Width * background.Height];

            byte alphaWeight = alpha;
            byte inverseAlphaWeight = (byte)(255 - alpha);

            int bgWidth = (int)background.Width;
            int bgHeight = (int)background.Height;
            int imgWidth = (int)background.Width;
            int imgHeight = (int)background.Height;

            // Precompute alpha ratios using bit shifts
            float alphaWeightRatio = alphaWeight / 255f;
            float inverseAlphaWeightRatio = inverseAlphaWeight / 255f;

            for (int y = 0; y < bgHeight; y++)
            {
                for (int x = 0; x < bgWidth; x++)
                {
                    int bgIndex = (y * bgWidth) + x;
                    int imgIndex = (y < imgHeight && x < imgWidth) ? (y * imgWidth) + x : -1;

                    int bgColor = background.RawData[bgIndex];
                    int imgColor = imgIndex >= 0 ? rawData[imgIndex] : bgColor;

                    byte bgAlpha = (byte)((bgColor >> 24) & 0xFF);
                    byte bgRed = (byte)((bgColor >> 16) & 0xFF);
                    byte bgGreen = (byte)((bgColor >> 8) & 0xFF);
                    byte bgBlue = (byte)(bgColor & 0xFF);

                    byte imgAlpha = (byte)((imgColor >> 24) & 0xFF);
                    byte imgRed = (byte)((imgColor >> 16) & 0xFF);
                    byte imgGreen = (byte)((imgColor >> 8) & 0xFF);
                    byte imgBlue = (byte)(imgColor & 0xFF);

                    byte resultAlpha;
                    byte resultRed;
                    byte resultGreen;
                    byte resultBlue;

                    if (blendImageAlpha)
                    {
                        if (imgAlpha == 0)
                        {
                            resultAlpha = bgAlpha;
                            resultRed = bgRed;
                            resultGreen = bgGreen;
                            resultBlue = bgBlue;
                        }
                        else
                        {
                            // Precompute normalized alphas
                            float imgAlphaNormalized = imgAlpha / 255f;
                            float bgAlphaNormalized = bgAlpha / 255f;

                            // Precompute the blending factors
                            float imgAlphaWeight = alphaWeightRatio * imgAlphaNormalized;
                            float bgAlphaWeight = inverseAlphaWeightRatio * bgAlphaNormalized;

                            resultAlpha = (byte)(Math.Min(1.0f, imgAlphaWeight + bgAlphaWeight) * 255);
                            resultRed = (byte)((((imgAlphaNormalized * imgRed) + ((1 - imgAlphaNormalized) * bgRed)) * alphaWeightRatio) + (bgRed * inverseAlphaWeightRatio));
                            resultGreen = (byte)((((imgAlphaNormalized * imgGreen) + ((1 - imgAlphaNormalized) * bgGreen)) * alphaWeightRatio) + (bgGreen * inverseAlphaWeightRatio));
                            resultBlue = (byte)((((imgAlphaNormalized * imgBlue) + ((1 - imgAlphaNormalized) * bgBlue)) * alphaWeightRatio) + (bgBlue * inverseAlphaWeightRatio));
                        }
                    }
                    else
                    {
                        resultAlpha = (byte)(((alphaWeight << 8) + (inverseAlphaWeight * bgAlpha)) >> 8);
                        resultRed = (byte)(((alphaWeight * imgRed) + (inverseAlphaWeight * bgRed)) >> 8);
                        resultGreen = (byte)(((alphaWeight * imgGreen) + (inverseAlphaWeight * bgGreen)) >> 8);
                        resultBlue = (byte)(((alphaWeight * imgBlue) + (inverseAlphaWeight * bgBlue)) >> 8);
                    }

                    blendedData[bgIndex] = (resultAlpha << 24) | (resultRed << 16) | (resultGreen << 8) | resultBlue;
                }
            }

            blendedBitmap.RawData = blendedData;

            return blendedBitmap;
        }

        public static int[] Blend(this int[] background, int[] rawData, uint width, uint height, byte alpha, bool blendImageAlpha)
        {
            // Create a new bitmap for the blended result with the size of the background

            int[] blendedData = new int[width * height];

            byte alphaWeight = alpha;
            byte inverseAlphaWeight = (byte)(255 - alpha);

            int bgWidth = (int)width;
            int bgHeight = (int)height;
            int imgWidth = (int)width;
            int imgHeight = (int)height;

            // Precompute alpha ratios using bit shifts
            float alphaWeightRatio = alphaWeight / 255f;
            float inverseAlphaWeightRatio = inverseAlphaWeight / 255f;

            for (int y = 0; y < bgHeight; y++)
            {
                for (int x = 0; x < bgWidth; x++)
                {
                    int bgIndex = (y * bgWidth) + x;
                    int imgIndex = (y < imgHeight && x < imgWidth) ? (y * imgWidth) + x : -1;

                    int bgColor = background[bgIndex];
                    int imgColor = imgIndex >= 0 ? rawData[imgIndex] : bgColor;

                    byte bgAlpha = (byte)((bgColor >> 24) & 0xFF);
                    byte bgRed = (byte)((bgColor >> 16) & 0xFF);
                    byte bgGreen = (byte)((bgColor >> 8) & 0xFF);
                    byte bgBlue = (byte)(bgColor & 0xFF);

                    byte imgAlpha = (byte)((imgColor >> 24) & 0xFF);
                    byte imgRed = (byte)((imgColor >> 16) & 0xFF);
                    byte imgGreen = (byte)((imgColor >> 8) & 0xFF);
                    byte imgBlue = (byte)(imgColor & 0xFF);

                    byte resultAlpha;
                    byte resultRed;
                    byte resultGreen;
                    byte resultBlue;

                    if (blendImageAlpha)
                    {
                        if (imgAlpha == 0)
                        {
                            resultAlpha = bgAlpha;
                            resultRed = bgRed;
                            resultGreen = bgGreen;
                            resultBlue = bgBlue;
                        }
                        else
                        {
                            // Precompute normalized alphas
                            float imgAlphaNormalized = imgAlpha / 255f;
                            float bgAlphaNormalized = bgAlpha / 255f;

                            // Precompute the blending factors
                            float imgAlphaWeight = alphaWeightRatio * imgAlphaNormalized;
                            float bgAlphaWeight = inverseAlphaWeightRatio * bgAlphaNormalized;

                            resultAlpha = (byte)(Math.Min(1.0f, imgAlphaWeight + bgAlphaWeight) * 255);
                            resultRed = (byte)((((imgAlphaNormalized * imgRed) + ((1 - imgAlphaNormalized) * bgRed)) * alphaWeightRatio) + (bgRed * inverseAlphaWeightRatio));
                            resultGreen = (byte)((((imgAlphaNormalized * imgGreen) + ((1 - imgAlphaNormalized) * bgGreen)) * alphaWeightRatio) + (bgGreen * inverseAlphaWeightRatio));
                            resultBlue = (byte)((((imgAlphaNormalized * imgBlue) + ((1 - imgAlphaNormalized) * bgBlue)) * alphaWeightRatio) + (bgBlue * inverseAlphaWeightRatio));
                        }
                    }
                    else
                    {
                        resultAlpha = (byte)(((alphaWeight << 8) + (inverseAlphaWeight * bgAlpha)) >> 8);
                        resultRed = (byte)(((alphaWeight * imgRed) + (inverseAlphaWeight * bgRed)) >> 8);
                        resultGreen = (byte)(((alphaWeight * imgGreen) + (inverseAlphaWeight * bgGreen)) >> 8);
                        resultBlue = (byte)(((alphaWeight * imgBlue) + (inverseAlphaWeight * bgBlue)) >> 8);
                    }

                    blendedData[bgIndex] = (resultAlpha << 24) | (resultRed << 16) | (resultGreen << 8) | resultBlue;
                }
            }

            return blendedData;
        }

        public static void DrawImagePart(this Bitmap bmp, Bitmap image, int x, int y, int w, int h, int ox = 0, int oy = 0)
        {
            for (int i = ox; i < w; i++)
            {
                for (int j = oy; j < h; j++)
                {
                    Color color = Color.FromArgb(image.RawData[i + (j * image.Width)]);
                    if (color.A > 0)
                    {
                        if (color.A < 255)
                        {
                            color = VBE.AlphaBlend(color, Color.FromArgb(bmp.RawData[x + i + ((y + j) * bmp.Width)]), color.A);
                        }
                        bmp.RawData[x + i + ((y + j) * bmp.Width)] = color.ToArgb();
                    }

                }
            }
        }
        public static void DrawCircle(this Bitmap bmp, int cx, int cy, int r, Color color)
        {
            // Boundaries of the bitmap
            int bmpWidth = (int)bmp.Width;
            int bmpHeight = (int)bmp.Height;

            // Midpoint circle algorithm
            int x = r;
            int y = 0;
            int err = 0;

            while (x >= y)
            {
                // Draw horizontal lines from left to right
                if (cx - x >= 0 && cx - x < bmpWidth && cy + y >= 0 && cy + y < bmpHeight)
                    bmp.DrawPixel(cx - x, cy + y, color);
                if (cx + x >= 0 && cx + x < bmpWidth && cy + y >= 0 && cy + y < bmpHeight)
                    bmp.DrawPixel(cx + x, cy + y, color);

                if (cx - y >= 0 && cx - y < bmpWidth && cy + x >= 0 && cy + x < bmpHeight)
                    bmp.DrawPixel(cx - y, cy + x, color);
                if (cx + y >= 0 && cx + y < bmpWidth && cy + x >= 0 && cy + x < bmpHeight)
                    bmp.DrawPixel(cx + y, cy + x, color);

                if (cx - x >= 0 && cx - x < bmpWidth && cy - y >= 0 && cy - y < bmpHeight)
                    bmp.DrawPixel(cx - x, cy - y, color);
                if (cx + x >= 0 && cx + x < bmpWidth && cy - y >= 0 && cy - y < bmpHeight)
                    bmp.DrawPixel(cx + x, cy - y, color);

                if (cx - y >= 0 && cx - y < bmpWidth && cy - x >= 0 && cy - x < bmpHeight)
                    bmp.DrawPixel(cx - y, cy - x, color);
                if (cx + y >= 0 && cx + y < bmpWidth && cy - x >= 0 && cy - x < bmpHeight)
                    bmp.DrawPixel(cx + y, cy - x, color);

                if (err <= 0)
                {
                    y += 1;
                    err += (2 * y) + 1;
                }
                if (err > 0)
                {
                    x -= 1;
                    err -= (2 * x) + 1;
                }
            }
        }


        public static Bitmap Resize(this Bitmap bmp, uint NewW, uint NewH)
        {
            if(bmp == null)
                return null;
            Bitmap bitmap = new Bitmap(NewW, NewH, ColorDepth.ColorDepth32)
            {
                RawData = ScaleImage(bmp, (int)NewW, (int)NewH)
            };
            return bitmap;
        }

        private static int[] ScaleImage(Image image, int newWidth, int newHeight)
        {
            int[] rawData = image.RawData;
            int width = (int)image.Width;
            int height = (int)image.Height;
            int[] array = new int[newWidth * newHeight];
            int num = ((width << 16) / newWidth) + 1;
            int num2 = ((height << 16) / newHeight) + 1;
            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    int num3 = (j * num) >> 16;
                    int num4 = (i * num2) >> 16;
                    array[(i * newWidth) + j] = rawData[(num4 * width) + num3];
                }
            }

            return array;
        }

        public static void DrawFullRoundedRectangle(this Bitmap bmp, int x, int y, int width, int height, int tlr, int trr, int brr, int blr, Color col)
        {
            // Draw the center rectangle
            bmp.DrawFilledRectangle(x + Math.Max(tlr, blr), y, width - Math.Max(tlr, blr) - Math.Max(trr, brr), height, col);
            bmp.DrawFilledRectangle(x, y + Math.Max(tlr, trr), Math.Max(tlr, blr), height - Math.Max(tlr, trr) - Math.Max(blr, brr), col);
            bmp.DrawFilledRectangle(x + width - Math.Max(trr, brr), y + Math.Max(trr, tlr), Math.Max(trr, brr), height - Math.Max(trr, tlr) - Math.Max(brr, blr), col);

            // Draw the top rectangle
            bmp.DrawFilledRectangle(x + tlr, y, width - tlr - trr, Math.Max(tlr, trr), col);

            // Draw the bottom rectangle
            bmp.DrawFilledRectangle(x + blr, y + height - Math.Max(blr, brr), width - blr - brr, Math.Max(blr, brr), col);

            // Draw the left rectangle
            bmp.DrawFilledRectangle(x, y + tlr, Math.Max(tlr, blr), height - tlr - blr, col);

            // Draw the right rectangle
            bmp.DrawFilledRectangle(x + width - trr, y + trr, trr, height - trr - brr, col);

            // Draw the top-left circle
            if (tlr > 0)
                bmp.DrawFilledCircle(col, x + tlr, y + tlr, tlr, tlr);

            // Draw the top-right circle
            if (trr > 0)
                bmp.DrawFilledCircle(col, x + width - trr - 1, y + trr, trr, trr);

            // Draw the bottom-right circle
            if (brr > 0)
                bmp.DrawFilledCircle(col, x + width - brr - 1, y + height - brr - 1, brr, brr);

            // Draw the bottom-left circle
            if (blr > 0)
                bmp.DrawFilledCircle(col, x + blr, y + height - blr - 1, blr, blr);
        }

    }
}
