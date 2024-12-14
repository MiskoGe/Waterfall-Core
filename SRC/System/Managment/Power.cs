using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waterfall.System.Graphics;
using Waterfall.System.Graphics.Components;
using Waterfall.System.Graphics.WaterfallGraphics;

namespace Waterfall.System.Managment
{
    public static class Power
    {
        static int AnimationState = 0;
        static Bitmap Screen;
        static Bitmap Black;
        static void DrawScreen(string text, PowerAction Action)
        {
            for (int i = 0; i < 20; i++)
            {
                if (AnimationState < 10)
                {
                    if (AnimationState == 0)
                    {
                        Screen = GUI.MainCanvas.GetImage(0, 0, (int)GUI.ScreenWidth, (int)GUI.ScreenHeight);
                        Black = new Bitmap(GUI.ScreenWidth, GUI.ScreenHeight, ColorDepth.ColorDepth32);
                        Black.Clear(Color.Black);
                    }
                    AnimationState++;
                    GUI.MainCanvas.DrawImage(Screen.Blend(Black, (byte)((float)AnimationState * 25.5f), false), 0, 0);
                    GUI.MainCanvas.Display();
                    Heap.Collect();
                }
                else if (AnimationState < 20)
                {
                   
                    AnimationState++;
                    GUI.MainCanvas.DrawImage(GUI.Wallpaper.Blend(Black, (byte)((float)(20 - AnimationState) * 25.5f), false), 0, 0);
                    GUI.MainCanvas.Display();
                    Heap.Collect();
                }
            }
            switch(Action)
            {
                case PowerAction.Shutdown:
                    Cosmos.System.Power.Shutdown();
                    break;
                case PowerAction.Reboot:
                    Cosmos.System.Power.Reboot();
                    break;
                default:
                    Cosmos.System.Power.Shutdown();
                    break;
            }
        }
        public static void CallShutdown()
        {
            Multithreading.InterruptManager.EndAll();
            DrawScreen("Shutting Down", PowerAction.Shutdown);
        }
        public static void CallReboot()
        {
            Multithreading.InterruptManager.EndAll();
            DrawScreen("Rebooting", PowerAction.Reboot);
        }
    }
    public enum PowerAction { Shutdown, Reboot };
}
