using Cosmos.Core;
using System;
using System.Drawing;
using Waterfall.System.Graphics;
using Waterfall.System.Processes;
using Waterfall.System.Security;

namespace Waterfall.System.Core.CLI
{
	public class CLIDrawing
	{
		public CLIhost cliHost;
		public void Clear()
		{
			GUI.MainCanvas.Clear();
			cliHost.Lines.Clear();
		}
		public void ClearWithoutHeader()
		{
			GUI.MainCanvas.DrawFilledRectangle(Color.Black, 0, cliHost.Font.Height, (int)GUI.ScreenWidth, (int)GUI.ScreenHeight - cliHost.Font.Height, false);
			cliHost.CurrentLine = 1;
			cliHost.CurrentChar = 0;
			cliHost.Input.CurrChar = 0;
		}
		public void ClearRaw(int line)
		{
			GUI.MainCanvas.DrawFilledRectangle(Color.Black, 0, line * cliHost.Font.Height, (int)GUI.ScreenWidth, cliHost.Font.Height, false);
			cliHost.Lines[line] = new CLILine();
		}
		public void Clear(int line)
		{
			int lineReady = line - cliHost.startLine;
			GUI.MainCanvas.DrawFilledRectangle(Color.Black, 0, lineReady * cliHost.Font.Height, (int)GUI.ScreenWidth, cliHost.Font.Height, false);
			cliHost.Lines[line] = new CLILine();
		}
		public void Clear(int line, int charIndex)
		{
			int lineReady = line - cliHost.startLine;
			GUI.MainCanvas.DrawFilledRectangle(Color.Black, charIndex * cliHost.Font.Width, lineReady * cliHost.Font.Height, cliHost.Font.Width, cliHost.Font.Height, false);
		}
		public void ClearDrawnText()
		{
			int lineReady = cliHost.CurrentLine - cliHost.startLine;
			int startX = cliHost.CurrentChar * cliHost.Font.Width;
			if (startX < 0)
				startX = 0;
			GUI.MainCanvas.DrawFilledRectangle(Color.Black, startX, lineReady * cliHost.Font.Height, (int)GUI.ScreenWidth - startX, cliHost.Font.Height, false);
		}
		public void DrawInput()
		{
			if (cliHost.lastIndex != cliHost.Input.CurrChar)
			{
				cliHost.lastIndex = cliHost.Input.CurrChar;
				ClearDrawnText();
				string toDraw = "_";
				if (cliHost.Input.CurrChar >= 0 && cliHost.Input.CurrChar <= cliHost.Input.CurrentInput.Length)
				{
					string firstPart = cliHost.Input.CurrentInput.Substring(0, cliHost.Input.CurrChar);
					string secondPart = cliHost.Input.CurrentInput.Substring(cliHost.Input.CurrChar);
					toDraw = firstPart + "_" + secondPart;
				}
				Draw(toDraw, cliHost.Input.CurrentInput);
			}
		}
		public void DrawPath()
		{
			cliHost.CurrentColor = cliHost.CLIColors.CLIGreen;
			Write($"{LoginManager.Username}@Waterfall", false);
			cliHost.CurrentColor = cliHost.CLIColors.CLIGray;
			Write($":", false);
			cliHost.CurrentColor = cliHost.CLIColors.CLIBlue;
			string slashPath = cliHost.Path.Replace('\\', '/');
			Write($"{slashPath}", false);
			cliHost.CurrentColor = cliHost.CLIColors.CLIGray;
			Write($"$ ", false);
			Write(""); //create new token
		}
		public void DrawTop()
		{
			DrawCenteredText("===== Waterfall - Watershell CLI " + DateTime.Now.ToString() + " =====", 0);
		}
		public void DrawCustomTop(string text)
		{
			DrawCenteredText($"===== Waterfall - {text} =====", 0);
		}

		public void Draw(string text)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, cliHost.CurrentChar * cliHost.Font.Width, line * cliHost.Font.Height);
			GUI.MainCanvas.Display();
			cliHost.Lines[cliHost.CurrentLine].elements[cliHost.Lines[cliHost.CurrentLine].elements.Count - 1].text = text;
		}
		public void Draw(string text, string original)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, cliHost.CurrentChar * cliHost.Font.Width, line * cliHost.Font.Height);
			GUI.MainCanvas.Display();
			cliHost.Lines[cliHost.CurrentLine].elements[cliHost.Lines[cliHost.CurrentLine].elements.Count - 1].text = original;
		}
		public void Draw(string text, int charIndex)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, charIndex * cliHost.Font.Width, line * cliHost.Font.Height);
			GUI.MainCanvas.Display();
			if (text != "_")
			{
				if (cliHost.Lines[cliHost.CurrentLine].elements.Count == 0)
					cliHost.Lines[cliHost.CurrentLine].elements.Add(new CLIElement { col = cliHost.CurrentColor, text = text });
				cliHost.Lines[cliHost.CurrentLine].elements[cliHost.Lines[cliHost.CurrentLine].elements.Count - 1].text = text;
			}
		}
		public void Draw(string text, int charIndex, string original)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, charIndex * cliHost.Font.Width, line * cliHost.Font.Height);
			GUI.MainCanvas.Display();
			if (cliHost.Lines[cliHost.CurrentLine].elements.Count == 0)
				cliHost.Lines[cliHost.CurrentLine].elements.Add(new CLIElement { col = cliHost.CurrentColor, text = original });
			cliHost.Lines[cliHost.CurrentLine].elements[cliHost.Lines[cliHost.CurrentLine].elements.Count - 1].text = original;
		}
		public void Write(string text, bool update = true, bool edit = true)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, cliHost.CurrentChar * cliHost.Font.Width, line * cliHost.Font.Height);
			cliHost.CurrentChar += text.Length;
			if (edit)
			{
				if (update)
					GUI.MainCanvas.Display();
				cliHost.Lines[cliHost.CurrentLine].elements.Add(new CLIElement { text = text, col = cliHost.CurrentColor });
			}
		}
		public void WriteLine(string text)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, cliHost.CurrentChar * cliHost.Font.Width, line * cliHost.Font.Height);
			cliHost.Lines[cliHost.CurrentLine].elements.Add(new CLIElement { text = text, col = cliHost.CurrentColor });
			NextLine();
			GUI.MainCanvas.Display();
		}
		public void DrawCenteredText(string text)
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			int width = text.Length * cliHost.Font.Width;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, ((int)GUI.ScreenWidth - width) / 2, line * cliHost.Font.Height);
			cliHost.CurrentChar = 0;
			cliHost.Input.CurrChar = 0;
			cliHost.CurrentLine++;
			GUI.MainCanvas.Display();
		}
		public void DrawCenteredText(string text, int rawY)
		{
			int width = text.Length * cliHost.Font.Width;
			GUI.MainCanvas.DrawString(text, cliHost.Font, cliHost.CurrentColor, ((int)GUI.ScreenWidth - width) / 2, rawY);
			GUI.MainCanvas.Display();
		}
		public void ClearCursor()
		{
			int line = cliHost.CurrentLine - cliHost.startLine;
			Clear(line, cliHost.CurrentChar + cliHost.Input.CurrChar);
		}
		public void NextLine()
		{
			cliHost.CurrentLine++;
			cliHost.CurrentChar = 0;
			cliHost.Input.CurrChar = 0;
			cliHost.Lines.Add(new CLILine());
			HandleMaxLines();
		}
		public void HandleMaxLines()
		{
			if ((cliHost.CurrentLine - cliHost.startLine) >= cliHost.MaxLines)
			{
				cliHost.startLine = cliHost.CurrentLine - cliHost.MaxLines;
				RedrawEverything(cliHost.startLine);
			}
			else
				RedrawEverything(cliHost.startLine); //we want to make sure '_' is destroyed from this pla... canvas.
		}

		public void RedrawEverything(int startIndex)
		{
			try
			{
				GUI.MainCanvas.DrawFilledRectangle(Color.Black, 0, cliHost.Font.Height, (int)GUI.ScreenWidth, (int)GUI.ScreenHeight - cliHost.Font.Height, false);
				cliHost.CurrentLine = startIndex;
				for (int i = startIndex; i < cliHost.Lines.Count - 1; i++)
				{
					if (i - startIndex != 0) //For now lets just ignore RadianceOS 7 CLI header
					{
						cliHost.CurrentChar = 0;
						for (int j = 0; j < cliHost.Lines[i].elements.Count; j++)
						{
							cliHost.CurrentColor = cliHost.Lines[i].elements[j].col;
							Write(cliHost.Lines[i].elements[j].text, false, false);
						}
					}
					cliHost.CurrentLine++;
					cliHost.CurrentChar = 0;
					cliHost.Input.CurrChar = 0;
				}

			}
			catch (Exception ex)
			{
				Crash(ex);
			}
		}
		public void Crash(Exception ex)
		{
			Clear();
			cliHost.CurrentColor = cliHost.CLIColors.CLIRed;
			WriteLine("Error" + ex.ToString());
			GUI.MainCanvas.Display();
			while (true)
			{
				CPU.Halt();
			}
		}
	}
}
