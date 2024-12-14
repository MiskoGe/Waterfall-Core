using System.Drawing;
using Waterfall.System.Processes;

namespace Waterfall.System.Core.Waterbash.BashExecutors
{
	public class CLIBash : Watershell
	{
		public CLIhost cliHost;
		public override void CWriteLine(string text)
		{
			cliHost.CLIDrawing.WriteLine(text);
		}
		public override void CWrite(string text)
		{
			cliHost.CLIDrawing.Write(text);
		}
		public override void CChangeColor(Color color)
		{
			cliHost.CurrentColor = color;
		}
		public override void ChangePath(string path)
		{
			cliHost.Path = path;
		}
		public override string GetPath()
		{
			return cliHost.Path;
		}
		public override string GetDrive()
		{
			return cliHost.Drive;
		}
		public override void FinishCustomInput()
		{
			toUpdate = null;
			cliHost.CLIDrawing.DrawPath();
		}


		public override Color GetColor(ConsoleColor color)
		{
			switch (color)
			{
				case ConsoleColor.Red:
					return cliHost.CLIColors.CLIRed;
				case ConsoleColor.Green:
					return cliHost.CLIColors.CLIGreen;
				case ConsoleColor.Blue:
					return cliHost.CLIColors.CLIBlue;
				case ConsoleColor.Gray:
					return cliHost.CLIColors.CLIGray;
				case ConsoleColor.Yellow:
					return cliHost.CLIColors.CLIYellow;
                case ConsoleColor.Cyan:
                    return cliHost.CLIColors.CLICyan;
                default:
					return cliHost.CLIColors.CLIGray;
			}
		}

		public void ExecuteCommand(string command)
		{
			cliHost.CLIDrawing.ClearCursor();
			//if (!cliHost.CLILogin.LoginInProcess)
			cliHost.CLIDrawing.NextLine();
			Execute(command, cliHost);
			cliHost.CurrentChar = 0;
			cliHost.Input.CurrChar = 0;
			cliHost.Input.CurrentInput = "";
			if (/*!cliHost.CLILogin.LoginInProcess &&*/ cliHost.Focused && toUpdate == null)
				cliHost.CLIDrawing.DrawPath();
			else if (!cliHost.Focused || toUpdate != null)
			{
				cliHost.CLIDrawing.Write("");
			}
		}
	}
}
