using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using Waterfall.System.Security.Runtime;

namespace Waterfall.System.Core.Waterbash.Commands
{
	public class partition : WSHCommand
	{
        public override string HelpNote { get; set; } = "Manages partitions";
        public partition()
		{
			paramActions = new Dictionary<string, Action<Watershell>>
			{
				{ "-d", ParamD },
				{ "--disk", ParamD },
				{ "-p", ParamP },
				{ "--partition", ParamP },
				{ "-i", ParamI },
				{ "--info", ParamI },
				{ "-s", ParamS },
				{ "--size", ParamS },
				{ "-c", ParamC },
				{ "--create", ParamC },
				{ "-f", ParamF },
				{ "--format", ParamF },
				{ "--help", ParamHelp },
				{ "-e", ParamE },
				{ "--erease", ParamE },
			};
			paramPriorities = new Dictionary<string, int>
			{
				{ "--help", 0 },
				{ "-p", 1 },
				{ "--partition", 1 },
				{ "-d", 0 },
				{ "--disk", 0 },
				{ "-i", 2 },
				{ "--info", 2 },
				{ "-s", 2 },
				{ "--size", 2 },
				{ "-c", 3 },
				{ "--create", 3 },
				{ "-e", 3 },
				{ "--erease", 3 },
				{ "-f", 4 },
				{ "--format", 4 },
			};
			paramHelp = new List<ParamInfo>
			{
				new ParamInfo{ longParam = "disk", shortParam = 'd', description = "select a disk", bonusParams = "disk number" },
				new ParamInfo{ longParam = "partition", shortParam = 'p', description = "select a partition", bonusParams = "partition number" },
				new ParamInfo{ longParam = "info", shortParam = 'i', description = "print selected partition information", },
				new ParamInfo{ longParam = "size", shortParam = 's', description = "set new partition size", bonusParams = "partition size in MiB" },
				new ParamInfo{ longParam = "create", shortParam = 'c', description = "create a new partition" },
				new ParamInfo{ longParam = "erease", shortParam = 'e', description = "erease selected partition" },
				new ParamInfo{ longParam = "format", shortParam = 'f', description = "format *already existing* selected partition", bonusParams = "FAT32/FAT16/FAT12" },
				new ParamInfo{ longParam = "help", description = "display this help" },
			};

		}

		int currentParam;
		string[] myParams;
		int Disk;
		int Partition;
		int Size;
		bool notifed;
		int command;
		string formatAs;
		public override void Execute(string[] Params, Watershell myBash)
		{
			if (!GlobalConfig.VFSInitialized)
			{
				myBash.CWriteLine($"VFS has not been initialized. Operation could not be performed.");
				return;
			}
			if (Params.Length == 0)
			{
				ParamI(myBash);
			}
			else
			{
				Params = SortParams(Params);
				myParams = Params;
				for (currentParam = 0; currentParam < Params.Length; currentParam++)
				{
					if (paramActions.ContainsKey(Params[currentParam]))
						paramActions[Params[currentParam]](myBash);
					else
					{
						myBash.CWriteLine($"Unknown parameter: {Params[currentParam]}");
					}
				}
			}
		}
		void ParamP(Watershell myBash)
		{
			Partition = Convert.ToInt32(myParams[currentParam += 1]);
			if (Kernel.fs.Disks[Disk].Partitions.Count <= Partition)
			{
				myBash.CWriteLine($"Partition {Partition} does not exist.");
				Partition = 0;
				return;
			}
		}
		void ParamD(Watershell myBash)
		{
			Disk = Convert.ToInt32(myParams[currentParam += 1]);
		}
		void ParamS(Watershell myBash)
		{
			Size = Convert.ToInt32(myParams[currentParam += 1]);
		}
		void ParamI(Watershell myBash)
		{
			int partitionIndex = Partition;
			if (Kernel.fs.Disks[Disk].Partitions.Count == 0)
			{
				myBash.CWriteLine("No partitions found!");
				return;
			}

			ManagedPartition partition = Kernel.fs.Disks[Disk].Partitions[partitionIndex];
			myBash.CWrite($"Partition {partitionIndex}");
			myBash.CWrite(new string(' ', 20 - $"Partition {partitionIndex}".Length));
			myBash.CWrite($"Partition Size: {partition.Host.BlockSize * partition.Host.BlockCount / 1024 / 1024} MiB");
			myBash.CWrite(new string(' ', 30 - $"Partition Size: {partition.Host.BlockSize * partition.Host.BlockCount / 1024 / 1024} MiB".Length));
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
			myBash.CWrite($"Block Size: {partition.Host.BlockSize} Bytes");
			//	myBash.CWrite(new string(' ', 30 - $"Block Size: {(partition.Host.BlockSize)} Bytes".Length));
			myBash.CWriteLine("");
		}
		void ParamC(Watershell myBash)
		{
			if (Size < 4)
			{
				myBash.CWriteLine($"Cannot create a partition smaller than 4 MiB.");
				myBash.CWriteLine("");
				return;
			}
			myBash.CWriteLine($"Creating partition on disk {Disk}. Partition size: {Size} MiB.");
			command = 1;
			notifed = false;
			myBash.toUpdate = this;
		}

		void ParamF(Watershell myBash)
		{
			int partitionIndex = Partition;
			if (Kernel.fs.Disks[Disk].Partitions.Count == 0)
			{
				myBash.CWriteLine("No partitions found!");
				return;
			}
			string type = myParams[currentParam + 1];
			type = type.ToUpper();
			type = type.Trim();
			currentParam += 1;
			if (type != "FAT32" && type != "FAT16" && type != "FAT12")
			{
				myBash.CWriteLine($"Format {type} is not supported.");
				return;
			}
			formatAs = type;
			myBash.CWriteLine($"Formatting partition {Partition} on disk {Disk} to {type}.");
			command = 2;
			notifed = false;
			myBash.toUpdate = this;
		}

		void ParamE(Watershell myBash)
		{
			if (Kernel.fs.Disks[Disk].Partitions.Count == 0)
			{
				myBash.CWriteLine("No partitions found!");
				return;
			}
			myBash.CWriteLine($"Deleting partition {Partition} on disk {Disk}.");
			command = 3;
			notifed = false;
			myBash.toUpdate = this;
		}


		public override void Run(Watershell myShell)
		{
			if (!notifed)
			{
				myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Yellow));
				myShell.CWriteLine($"Are you sure you want to make the above changes to your disk?");
				myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
				myShell.CWriteLine($"y/n");
				myShell.CWrite("");
				notifed = true;
			}
		}

		public override void HandleOwnInput(Watershell myShell, string content)
		{
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
										if (Kernel.fs.Disks[Disk].IsMBR)
											Kernel.fs.Disks[Disk].CreatePartition(Size);
										else
										{
											ulong startingBlock = 34;
											for (int i = 0; i < Kernel.fs.Disks[Disk].Partitions.Count; i++)
											{
												startingBlock += Kernel.fs.Disks[Disk].Partitions[i].Host.BlockCount;
											}
											Cosmares.Setup.CreateGPTpartition(Kernel.fs.Disks[Disk], startingBlock, (ulong)Size);
										}
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
										myShell.CWriteLine("");
										myShell.CWriteLine($"New partition created successfully");
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
									catch (Exception ex)
									{
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
										myShell.CWriteLine("");
										myShell.CWriteLine(ex.Message);
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
								}
								break;
							case 2:
								{
									try
									{
										Kernel.fs.Disks[Disk].FormatPartition(Partition, formatAs, true);
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
										myShell.CWriteLine("");
										myShell.CWriteLine($"Partition has been formatted.");
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
									catch (Exception ex)
									{
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
										myShell.CWriteLine("");
										myShell.CWriteLine(ex.Message);
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
								}
								break;
							case 3:
								{
									try
									{
										if (Kernel.fs.Disks[Disk].IsMBR)
											Kernel.fs.Disks[Disk].DeletePartition(Partition);
										else
											Cosmares.Setup.DeleteGPTpartition(Kernel.fs.Disks[Disk], (uint)Partition);
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Green));
										myShell.CWriteLine("");
										myShell.CWriteLine($"Partition has been deleted.");
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
									catch (Exception ex)
									{
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
										myShell.CWriteLine("");
										myShell.CWriteLine(ex.Message);
										myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
									}
								}
								break;
						}
						myShell.FinishCustomInput();
					}
					break;
				case "n":
				case "no":
					{
						myShell.CWriteLine("");
						myShell.FinishCustomInput();
					}
					break;
			}
		}

		public void ParamHelp(Watershell myBash)
		{
			new HelpDisplayer(myBash, "partition [OPTION]...", paramHelp);
		}
	}
}
