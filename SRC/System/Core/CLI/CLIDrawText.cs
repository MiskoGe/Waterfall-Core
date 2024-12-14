using Waterfall.System.Core.Waterbash;

namespace Waterfall.System.Core.CLI
{
	public static class CLIDrawText
	{
		public static void DrawMassText(string text, Watershell shell)
		{
			string[] Lines = text.Split('\n');
			for (int i = 0; i < Lines.Length; i++)
			{
				shell.CWriteLine(Lines[i].Replace("\r", ""));
			}
		}
	}
}
