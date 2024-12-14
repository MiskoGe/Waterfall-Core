using System;


namespace Waterfall.System.Core.SystemInput
{
    public static class Mouse
    {
        public static int X { get; set; }
        public static int Y { get; set; }
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        public static MState MouseState { get; set; }
        public static void OnMouseChanged(int xDeltaX, int xDeltaY, int xMouseState, int xScrollWheel)
        {
            X = Math.Max(0, Math.Min(ScreenWidth, X + xDeltaX));
            Y = Math.Max(0, Math.Min(ScreenHeight, Y + xDeltaY));
            MouseState = (MState)xMouseState;
        }
        [Flags]
        public enum MState
        {
            None = 0,
            Left = 1,
            Right = 2,
            Middle = 4,
            FourthButton = 8,
            FifthButton = 0x10
        }
    }
}
