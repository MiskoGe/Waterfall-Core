using Waterfall.System.Core.SystemInput;
using Waterfall.System.Core.SystemInput.Keys;
using Waterfall.System.Graphics;
using Waterfall.System.Security;

namespace Waterfall.System.Core.Drivers.PS2
{
    public static class PS2KeyboardMouse
    {
        public static bool stopped;
        public static void Update()
        {
            return;
            if (!KeyboardInput.AlreadyUsed && GUI.Wallpaper != null && !stopped)
            {
                KeyHandler.KeyboardAcceleration();
                while (KeyHandler.KeyAvailable)
                {
                    KeyInfo key = KeyHandler.ReadKey();
                    switch (key.Key)
                    {
                        case KeyboardKey.D:
                            Mouse.X += 6;
                            break;
                        case KeyboardKey.W:
                            Mouse.Y -= 6;
                            break;
                        case KeyboardKey.S:
                            Mouse.Y += 6;
                            break;
                        case KeyboardKey.A:
                            Mouse.X -= 6;
                            break;
                        case KeyboardKey.Q:
                            if (Mouse.MouseState == Mouse.MState.None)
                                Mouse.MouseState = Mouse.MState.Left;
                            else
                                Mouse.MouseState = Mouse.MState.None;
                            break;
                        case KeyboardKey.E:
                            if (Mouse.MouseState == Mouse.MState.None)
                                Mouse.MouseState = Mouse.MState.Right;
                            else
                                Mouse.MouseState = Mouse.MState.None;
                            break;
                        case KeyboardKey.C:
                            stopped = true;
                            break;
                    }
                }


            }
        }
    }
}
