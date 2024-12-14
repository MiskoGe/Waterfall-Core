using Waterfall.System.Core.CLI;
using Waterfall.System.Core.Drivers.PS2;

namespace Waterfall.System.Drivers
{
	public static class DriversManager
	{
		public static PS2Controller PS2Controller { get; set; }
		public static void StartupPS2(CLILogs logs)
		{
			PS2Controller = new PS2Controller { logs = logs };
			PS2Controller.Initialize(true);
		}
	}
}
