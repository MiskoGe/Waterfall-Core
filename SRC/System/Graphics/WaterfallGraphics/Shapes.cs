using System;
using System.Drawing;

namespace Waterfall.System.Graphics.WaterfallGraphics
{
    public static class Shapes
    {
        public static void DrawFullRoundedRectangle(int x, int y, int width, int height, int tlr, int trr, int brr, int blr, Color col)
        {
            // Draw the center rectangle
            GUI.MainCanvas.DrawFilledRectangle(col, x + Math.Max(tlr, blr), y, width - Math.Max(tlr, blr) - Math.Max(trr, brr), height);
            GUI.MainCanvas.DrawFilledRectangle(col, x, y + Math.Max(tlr, trr), Math.Max(tlr, blr), height - Math.Max(tlr, trr) - Math.Max(blr, brr));
            GUI.MainCanvas.DrawFilledRectangle(col, x + width - Math.Max(trr, brr), y + Math.Max(trr, tlr), Math.Max(trr, brr), height - Math.Max(trr, tlr) - Math.Max(brr, blr));

            // Draw the top rectangle
            GUI.MainCanvas.DrawFilledRectangle(col, x + tlr, y, width - tlr - trr, Math.Max(tlr, trr));

            // Draw the bottom rectangle
            GUI.MainCanvas.DrawFilledRectangle(col, x + blr, y + height - Math.Max(blr, brr), width - blr - brr, Math.Max(blr, brr));

            // Draw the left rectangle
            GUI.MainCanvas.DrawFilledRectangle(col, x, y + tlr, Math.Max(tlr, blr), height - tlr - blr);

            // Draw the right rectangle
            GUI.MainCanvas.DrawFilledRectangle(col, x + width - trr, y + trr, trr, height - trr - brr);

            // Draw the top-left circle
            if (tlr > 0)
                GUI.MainCanvas.DrawFilledCircle(col, x + tlr, y + tlr, tlr);

            // Draw the top-right circle
            if (trr > 0)
                GUI.MainCanvas.DrawFilledCircle(col, x + width - trr - 1, y + trr, trr);

            // Draw the bottom-right circle
            if (brr > 0)
                GUI.MainCanvas.DrawFilledCircle(col, x + width - brr - 1, y + height - brr - 1, brr);

            // Draw the bottom-left circle
            if (blr > 0)
                GUI.MainCanvas.DrawFilledCircle(col, x + blr, y + height - blr - 1, blr);
        }

    }
}
