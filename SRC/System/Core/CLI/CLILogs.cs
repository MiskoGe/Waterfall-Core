using System.Drawing;
using Waterfall.System.Processes;

namespace Waterfall.System.Core.CLI
{
	public class CLILogs
	{
		public CLIhost host;
		public void WriteOk(string message)
		{
			Color cachedColor = host.CurrentColor;
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.Write("[");
			host.CurrentColor = host.CLIColors.CLIGreen;
			host.CLIDrawing.Write("  OK  ");
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.WriteLine($"] {message}");
			host.CurrentColor = cachedColor;
		}
		public void WriteInfo(string message)
		{
			Color cachedColor = host.CurrentColor;
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.Write("[");
			host.CurrentColor = host.CLIColors.CLICyan;
			host.CLIDrawing.Write(" INFO ");
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.WriteLine($"] {message}");
			host.CurrentColor = cachedColor;
		}
		public void WriteWarn(string message)
		{
			Color cachedColor = host.CurrentColor;
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.Write("[");
			host.CurrentColor = host.CLIColors.CLIYellow;
			host.CLIDrawing.Write(" WARN ");
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.WriteLine($"] {message}");
			host.CurrentColor = cachedColor;
		}
		public void WriteError(string message)
		{
			Color cachedColor = host.CurrentColor;
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.Write("[");
			host.CurrentColor = host.CLIColors.CLIRed;
			host.CLIDrawing.Write(" FAIL ");
			host.CurrentColor = host.CLIColors.CLIGray;
			host.CLIDrawing.WriteLine($"] {message}");
			host.CurrentColor = cachedColor;
		}
	}
}
