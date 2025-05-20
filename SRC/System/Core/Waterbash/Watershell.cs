using System;
using System.Collections.Generic;
using System.Drawing;
using Waterfall.System.Core.ProcessManager;
using Waterfall.System.Core.Waterbash.Commands;

namespace Waterfall.System.Core.Waterbash
{
	/// <summary>
	/// Represents a Bash like shell implementation.
	/// </summary>
	public class Watershell
	{
		/// <summary>
		/// Writes text to the console.
		/// </summary>
		/// <param name="text">The text to write.</param>
		public virtual void CWrite(string text) { }

		/// <summary>
		/// Writes a line of text to the console.
		/// </summary>
		/// <param name="text">The text to write.</param>
		public virtual void CWriteLine(string text) { }

		/// <summary>
		/// Changes the console text color.
		/// </summary>
		/// <param name="color">The color to change to.</param>
		public virtual void CChangeColor(Color color) { }

		/// <summary>
		/// Changes the current path.
		/// </summary>
		/// <param name="path">The new path.</param>
		public virtual void ChangePath(string path) { }

		/// <summary>
		/// Gets the current path.
		/// </summary>
		/// <returns>The current path.</returns>
		public virtual string GetPath() { return null; }

		/// <summary>
		/// Gets the current drive name
		/// </summary>
		/// <returns>Drive name.</returns>
		public virtual string GetDrive() { return null; }

		public virtual Color GetColor(ConsoleColor color) { return Color.Red; }

		public enum ConsoleColor { Red, Green, Blue, Yellow, Gray, Cyan }
		/// <summary>
		/// Delegate for creating BashCommand instances.
		/// </summary>
		public delegate WSHCommand CommandCreator();

		/// <summary>
		/// Dictionary of available commands and their creators.
		/// </summary>
		public Dictionary<string, CommandCreator> commands = new Dictionary<string, CommandCreator>();
		/// <summary>
		/// Watershell host process
		/// </summary>
		/// 
		public Process Process { get; set; }
		/// <summary>
		/// Generates the list of commands.
		/// </summary>
		public void Gen()
		{
            commands.Add("benchmark", () => new benchmark());
            commands.Add("cat", () => new cat());
			commands.Add("cd", () => new cd());
			commands.Add("del", () => new del());
			commands.Add("dir", () => new dir());
			commands.Add("disk", () => new disk());
			commands.Add("echo", () => new echo());
			commands.Add("fs", () => new fs());
            commands.Add("help", () => new help());
			commands.Add("md", () => new md());
			commands.Add("partition", () => new partition());
			commands.Add("poweroff", () => new poweroff());
			commands.Add("ram", () => new ram());
            commands.Add("rd", () => new rd());
            commands.Add("reboot", () => new reboot());
			commands.Add("touch", () => new touch());
			commands.Add("uname", () => new uname());
			commands.Add("zyntra", () => new zyntra());
        }

		/// <summary>
		/// commands to update
		/// </summary>
		public WSHCommand toUpdate;

		/// <summary>
		/// Stop writing to WSHCommand toUpdate and start to terminal's input
		/// </summary>
		public virtual void FinishCustomInput()
		{
			toUpdate = null;
		}

		/// <summary>
		/// Update command
		/// </summary>
		public void Update()
		{
			if (toUpdate != null)
			{
				toUpdate.Run(this);
			}
		}

		/// <summary>
		/// Handle commands input
		/// </summary>
		/// <param name="content"></param>
		public void HandleInput(string content)
		{
			toUpdate.HandleOwnInput(this, content);
		}

		/// <summary>
		/// Executes a command with the given process context.
		/// </summary>
		/// <param name="com">The command to execute.</param>
		/// <param name="process">The process context.</param>
		public void Execute(string com, Process process)
		{
			if (com == string.Empty)
				return;
			try
			{
				string[] commandParts = com.Split(' ');
				List<string> readyParts = new List<string>();
				foreach (string part in commandParts)
				{
					if (part != " " && part != string.Empty && part != null)
					{
						readyParts.Add(part);
					}
				}
				string commandName = readyParts[0].ToLower();
				string[] commandParams;
				if (readyParts.Count > 1)
				{
					commandParams = new string[readyParts.Count - 1];
					Array.Copy(readyParts.ToArray(), 1, commandParams, 0, commandParams.Length);
				}
				else
				{
					commandParams = new string[0];
				}

				if (commands.ContainsKey(commandName))
				{
					WSHCommand command = commands[commandName]();
					for (int i = 0; i < commandParams.Length; i++)
					{
						if (!commandParams[i].StartsWith("-"))
							continue;
						if (!command.IsAllowedParam(commandParams[i]))
						{
							CWriteLine($"{commandName}: invalid option -- '{commandParams[i]}'");
							if (!string.IsNullOrEmpty(command.HelpParam))
								CWriteLine($"Try '{commandName} {command.HelpParam}' for more information.");
							return;
						}
					}

					if (commandParams.Length >= command.MinimumParamsLength)
						command.Execute(commandParams, this);
					else
						CWriteLine($"Invalid command syntax.");
				}
				else
				{
					if (commandName != "")
						CWriteLine($"{commandName}: command not found");
				}
			}
			catch (Exception ex)
			{
				CWriteLine("Exception: " + ex.Message);
			}
		}
	}
}
