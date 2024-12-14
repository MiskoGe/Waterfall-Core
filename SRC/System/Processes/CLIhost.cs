using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using Waterfall.System.Core.CLI;
using Waterfall.System.Core.ProcessManager;
using Waterfall.System.Core.SystemInput.Inputs;
using Waterfall.System.Core.Waterbash.BashExecutors;
using Waterfall.System.Graphics;
using Waterfall.System.Managment;

namespace Waterfall.System.Processes
{
	public class CLIhost : Process
	{
		public PCScreenFont Font;
		public int Length;
		public int MaxLines;
		public int CurrentChar = 0;
		public int CurrentLine = 1;
		public int lastIndex = 0;
		public int FontWidth, FontHeight;
		bool drawCursor;
		public CLIDrawing CLIDrawing;
		//public CLILogin CLILogin;
		public CLIBash CLIBash;
		public CLIColors CLIColors;
		public string Path = @"";
		public string Drive = @"0";
		public bool Focused = true;
		public List<CLILine> Lines = new List<CLILine>();
		public int startLine = 0;
		public Color CurrentColor;
		public CLIInput Input;

		public override void LoadConfig()
		{
			CLIDrawing = new CLIDrawing { cliHost = this };
			//CLILogin = new CLILogin { cliHost = this };
			CLIBash = new CLIBash { cliHost = this, Process = this as Process };
			CLIColors = new CLIColors();
			Input = new CLIInput { myHost = this };
			CLIBash.Gen();
			Priority = Priority.Lowest;
			MaxLines = (int)Math.Floor((double)GUI.ScreenHeight / 16) - 1;

		}
		public override void Start()
		{
			ProcessType = Core.ProcessManager.Type.SystemProtected;
			Lines.Add(new CLILine());
			Lines.Add(new CLILine());
			Length = (int)GUI.ScreenWidth / Font.Width;
			LoadFont();
			CurrentColor = CLIColors.CLIGray;
			//CLILogin.TryLogin();
		}

		public void OnFinishLoading()
		{
			CLIDrawing.DrawTop();
			CLIDrawing.DrawPath();
		}

		public void LoadFont()
		{
			Font = PCScreenFont.LoadFont(Resources.zap_ext_light16);
			FontWidth = Font.Width;
			FontHeight = Font.Height;
		}

		public override void Run()
		{
			try
			{
				CLIBash.Update();
				DrawInput();
			}
			catch (Exception ex)
			{
				CLIDrawing.Crash(ex);
			}
		}

		public override void RunEverySecond()
		{
			CLIDrawing.ClearRaw(0);
			CLIDrawing.DrawTop();
		}
		ulong nextUpdate;
		public void DrawInput()
		{
			//if (!CLILogin.LoginInProcess)
			//{
			if (Focused)
			{
				Input.Monitor();
				CLIDrawing.DrawInput();
			}
			/*}
			else
			{
				InputSystem.Monitore(11, this, true, false, false, true, true, true);
				CLILogin.HandleLogin();
			}*/
			//}
			if (RealTime.CPUUptime >= nextUpdate)
			{
				if (!drawCursor)
				{
					CLIDrawing.Clear(CurrentLine, CurrentChar + Input.CurrChar);
					drawCursor = true;
				}
				else
				{
					CLIDrawing.Draw("_", CurrentChar + Input.CurrChar);
					drawCursor = false;
				}
				nextUpdate = RealTime.CPUUptime + (RealTime.CPUSecond / 2);
			}
		}
	}
}