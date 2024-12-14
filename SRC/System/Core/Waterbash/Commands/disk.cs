using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using Waterfall.System.Core.WaterfallVFS;
using Waterfall.System.Security.Runtime;

namespace Waterfall.System.Core.Waterbash.Commands
{
	public class disk : WSHCommand
	{
        public override string HelpNote { get; set; } = "Manages disks";
        bool notifed;
		int command;
		public disk()
		{
			paramActions = new Dictionary<string, Action<Watershell>>
			{
				{ "-l", ParamL },
				{ "--list", ParamL },
				{ "-i", ParamI },
				{ "--info", ParamI },
				{ "-d", ParamD },
				{ "--disk", ParamD },
				{ "-g", ParamG },
				{ "--gpt", ParamG },
				{ "--help", ParamHelp },
			};
			paramPriorities = new Dictionary<string, int>
			{
				{ "--help", 0 },
				{ "-l", 1 },
				{ "--list", 1 },
				{ "-i", 1 },
				{ "--info", 1 },
				{ "-d", 0 },
				{ "--disk", 0 },
				{ "-g", 2 },
				{ "--gpt", 2 },
			};
		}
		int currentParam;
		string[] myParams;
		int Disk;
		public override void Execute(string[] Params, Watershell myBash)
		{
			if (!GlobalConfig.VFSInitialized)
			{
				myBash.CWriteLine($"VFS has not been initialized. Operation could not be performed.");
				return;
			}
			myParams = Params;
			if (Params.Length == 0)
			{
				ParamL(myBash);
			}
			else
			{
				Params = SortParams(Params);
				for (currentParam = 0; currentParam < Params.Length; currentParam++)
				{
					if (paramActions.ContainsKey(Params[currentParam]))
						paramActions[Params[currentParam]](myBash);
					else
					{
						myBash.CWrite($"Unknown parameter: {Params[currentParam]}");
					}
				}
			}
		}
		void ParamD(Watershell myBash)
		{
			Disk = Convert.ToInt32(myParams[currentParam += 1]);
		}
		void ParamG(Watershell myBash)
		{
			if (Disks.Info[Disk].DiskType == DiskType.IDE)
			{
				myBash.CWriteLine($"Sorry! GPT IDE Drives are not supported yet.");
				myBash.CWriteLine($"Please switch to SATA drive.");
				return;
			}
			command = 1;
			notifed = false;
			myBash.toUpdate = this;
		}

		void ParamI(Watershell myBash)
		{
			try
			{
				int Disk = Convert.ToInt32(myParams[currentParam += 1]);
				myBash.CWriteLine($"Disk {Disk}");
				myBash.CWriteLine($"Partitions: {Kernel.fs.Disks[Disk].Partitions.Count}");
				for (int i = 0; i < Kernel.fs.Disks[Disk].Partitions.Count; i++)
				{
					ManagedPartition partition = Kernel.fs.Disks[Disk].Partitions[i];
					myBash.CWrite($"Partition {i}");
					myBash.CWrite(new string(' ', 20 - $"Partition {i}".Length));
					myBash.CWrite($"Formatted: {partition.HasFileSystem}");
					myBash.CWrite(new string(' ', 20 - $"Formatted: {partition.HasFileSystem}".Length));
					if (partition.HasFileSystem)
					{
						myBash.CWrite($"Root Path: {partition.RootPath}");
						myBash.CWrite(new string(' ', 20 - $"Root Path: {partition.RootPath}".Length));
					}
					else
					{
						myBash.CWrite($"Root Path: n/a");
						myBash.CWrite(new string(' ', 20 - $"Root Path: n/a".Length));
					}
					myBash.CWrite($"Partition Size: {partition.Host.BlockSize * partition.Host.BlockCount / 1024 / 1024} MiB");

					myBash.CWriteLine("");
				}
			}
			catch (Exception ex)
			{
				myBash.CWriteLine("Exception: " + ex.Message);
			}

		}
		void ParamL(Watershell myBash)
		{
			if (Kernel.fs.Disks.Count == 0)
			{
				myBash.CWriteLine("No disks found!");
				myBash.CWriteLine("Is the file system registered?");
			}
			for (int i = 0; i < Kernel.fs.Disks.Count; i++)
			{
				myBash.CWrite($"Disk {i}");
				myBash.CWrite(new string(' ', 20 - $"Disk {i}".Length));
				myBash.CWrite($"Size: {Kernel.fs.Disks[i].Size / (1024 * 1024)}MiB");
				myBash.CWrite(new string(' ', 20 - $"Size: {Kernel.fs.Disks[i].Size / (1024 * 1024)} MiB".Length));
				myBash.CWrite($"Partitions: {Kernel.fs.Disks[i].Partitions.Count}");
				myBash.CWrite(new string(' ', 20 - $"Partitions: {Kernel.fs.Disks[i].Partitions.Count}".Length));
				if (Kernel.fs.Disks[Disk].IsMBR)
					myBash.CWrite($"MBR");
				else
					myBash.CWrite($"GPT");
				myBash.CWrite(new string(' ', 10));
				switch (Disks.Info[i].DiskType)
				{
					case DiskType.IDE:
						myBash.CWrite($"IDE");
						break;
					case DiskType.SATA:
						myBash.CWrite($"SATA");
						break;
				}
				myBash.CWriteLine("");
			}
		}

		public override void Run(Watershell myShell)
		{
			switch (command)
			{
				case 1:
					{
						if (!notifed)
						{
							myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Yellow));
							myShell.CWriteLine($"Are you sure you want to switch disk {Disk} to GPT?");
							myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
							myShell.CWriteLine($"DON'T DO THIS ON REAL HARDWARE!");
							myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
							myShell.CWriteLine($"y/n");
							myShell.CWrite("");
							notifed = true;
						}
					}
					break;
			}
		}

		public override void HandleOwnInput(Watershell myShell, string content)
		{
			if (command == 2)
				return;
			switch (content)
			{
				case "y":
				case "yes":
					{
						switch (command)
						{
							case 1:
								{
									try
									{
										Cosmares.Setup.CreateGPT(Kernel.fs.Disks[Disk].Host);
										myShell.CWriteLine("");
										myShell.CWriteLine("Done!");
										myShell.FinishCustomInput();
									}
									catch (Exception ex)
									{
										notifed = false;
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
										myShell.CWriteLine("");
										myShell.CWriteLine(ex.Message);
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
								}
								break;
						}
					}
					break;
				case "n":
				case "no":
					{
						myShell.FinishCustomInput();
						myShell.CWriteLine("");
					}
					break;
			}
		}
		public void ParamHelp(Watershell myBash)
		{
			myBash.CWriteLine("Usage: disk [OPTION]...");
			myBash.CWriteLine("  -d, --disk <disk number> select a disk");
			myBash.CWriteLine("  -l, --list               print all disks");
			myBash.CWriteLine("  -i, --info               print information about selected disk");
			myBash.CWriteLine("  -g, --gpt                change selected MBR disk to GPT disk");
			myBash.CWriteLine("      --help               display this help");
		}
	}
}
