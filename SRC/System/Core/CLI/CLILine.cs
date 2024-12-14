using System.Collections.Generic;
using System.Drawing;

namespace Waterfall.System.Core.CLI
{
	public class CLILine
	{
		public List<CLIElement> elements = new List<CLIElement>();
	}
	public class CLIElement
	{
		public string text;
		public Color col;
	}
}
