
using Waterfall.System.Core.CLI;
using Waterfall.System.Core.ProcessManager;
using Waterfall.System.Drivers;
using Waterfall.System.Drivers.USB;
using Waterfall.System.Graphics;
using Waterfall.System.Processes;
using Waterfall.System.Security;
using Waterfall.System.Security.Runtime;

namespace Waterfall.System.Boot
{
	public static class WaterfallBoot
	{
		public static CLILogs CLILogs;
		public static void Boot()
		{
			GUI.StartGUI();
			CLIhost CLI;
			ProcessManager.Start(CLI = new CLIhost { });
			CLILogs = new CLILogs { host = CLI };
			CLI.CLIDrawing.DrawCustomTop("Booting up");
			CLILogs.WriteOk($"Loaded Waterfall CLI ");
			CLILogs.WriteOk($"GUI loaded with {GUI.MainCanvas.Name()} graphics driver. Mode: {GUI.MainCanvas.Mode}");
			DriversManager.StartupPS2(CLILogs);
			Hub.Find(CLILogs);

			CLILogs.WriteInfo($"Booted Waterfall {BuildConfig.Version} ({BuildConfig.SubVersion})");
			CLI.CLIBash.CWriteLine($"");
			CLI.CLIBash.CWriteLine($"Waterfall Operating System is licensed under BSD 3-Clause License");
			CLI.CLIBash.CWriteLine($"Copyright (c) 2024, Szymekk, SzymekkYT");
			CLI.CLIBash.CWriteLine($"All rights reserved.");
			CLI.CLIBash.CWriteLine($"");
            CLI.CLIBash.CWriteLine($"Welcome to Waterfall Core!");
            CLI.CLIBash.CWriteLine($"Type help to view Watefall command list.");
            CLI.CLIBash.CWriteLine($"");
            CLI.OnFinishLoading();
		}
	}
}
