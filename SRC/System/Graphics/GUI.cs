using Cosmos.Core;
using Cosmos.System.Graphics;
using System.Drawing;
using Waterfall.System.Core.SystemInput;
using Waterfall.System.Managment;

namespace Waterfall.System.Graphics
{
    public static class GUI
    {
        public static Canvas MainCanvas { get; set; }
        public static uint ScreenWidth { get; set; } = 1920;
        public static uint ScreenHeight { get; set; } = 1080;
        public static Colors colors = new Colors();
        public static bool StartWindows { get; set; } = false;
        public static Bitmap Wallpaper { get; set; }
        public static ulong NextRefresh;
        public static bool Clicked;
        public static bool RightClicked;
        static ulong nextRefresh;
        static ulong currentCPU;
        public static int MaxFPS = 0;
        public static bool EnableFPSLimit = true;
        public static bool ignoreDelayForNow;
        public static void VSync()
        {
            currentCPU = CPU.GetCPUUptime();

            if (currentCPU >= nextRefresh)
            {
                MainCanvas.Display();
                nextRefresh += RealTime.CPUSecond / (ulong)MaxFPS;
                RealTime.CountFPS();

                if (RealTime._fps > MaxFPS)
                {
                    ulong delay = (ulong)(RealTime._fps - MaxFPS) * (RealTime.CPUSecond / (ulong)MaxFPS);
                    if (!ignoreDelayForNow)
                    {
                        nextRefresh += delay;
                    }
                    else
                    {
                        if (delay < RealTime.CPUSecond / 10)
                        {
                            ignoreDelayForNow = false;
                        }
                    }
                }
            }
        }

        public static void StartGUI()
        {
            MainCanvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(ScreenWidth, ScreenHeight, Cosmos.System.Graphics.ColorDepth.ColorDepth32)); //SVGAII - VBE if not supported
                                                                                                                                                   //  MainCanvas = new VBECanvas(new Mode(ScreenWidth, ScreenHeight, Cosmos.System.Graphics.ColorDepth.ColorDepth32)); //Force VBE
            MainCanvas.Display();
            NextRefresh = CPU.GetCPUUptime();
        }
        public static Mouse.MState stateBefore;
        public static void UpdateGUI()
        {
            stateBefore = Mouse.MouseState;
            if (EnableFPSLimit && MaxFPS != 0)
                VSync();
            else
            {
                RealTime.CountFPS();
                MainCanvas.Display();
            }
            if (stateBefore == Mouse.MState.Left)
                Clicked = true;
            else if (Clicked)
                Clicked = false;

            if (stateBefore == Mouse.MState.Right)
                RightClicked = true;
            else if (RightClicked)
                RightClicked = false;
            KeyboardInput.AlreadyUsed = false;
        }
    }
}
