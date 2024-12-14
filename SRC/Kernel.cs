using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using System;
using System.Drawing;
using Waterfall.System.Boot;
using Waterfall.System.Core.ProcessManager;
using Waterfall.System.Graphics;
using Waterfall.System.Managment;
using Sys = Cosmos.System;

namespace Waterfall
{
	public class Kernel : Sys.Kernel
	{
		//Custom booting, without Cosmos PS2 controllers
		protected override void OnBoot() => Cosmos.System.Global.Init(GetTextScreen(), false, false, true, true);
		public static CosmosVFS fs;
	
		protected override void BeforeRun()
		{	
            WaterfallBoot.Boot();
        }
		protected override unsafe void Run()
		{
			try
			{
                RealTime.GetCPUUptime();
               // PS2KeyboardMouse.Update();
                ProcessManager.Update();
                GUI.UpdateGUI();
                RealTime.CallHeapCollect();
            }
			catch (Exception ex)
			{
				GUI.MainCanvas.Clear();
				GUI.MainCanvas.DrawString(ex.Message, Sys.Graphics.Fonts.PCScreenFont.Default, Color.Red, 0, 0);
				GUI.MainCanvas.Display();
			}
		}
	}
}
