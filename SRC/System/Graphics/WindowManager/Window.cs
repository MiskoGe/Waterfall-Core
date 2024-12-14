using System.Drawing;

namespace Waterfall.System.Graphics.WindowManager
{
	public class Window
	{
		public Rectangle Position { get; set; } = new Rectangle(100, 100, 100, 100);
		public string WindowName { get; set; } = "Window";
		public bool Closable { get; set; } = true;
	}
}
