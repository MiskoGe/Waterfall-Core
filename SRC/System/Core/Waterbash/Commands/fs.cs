using COROS;
using Cosmos.HAL.BlockDevice.Ports;
using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using Waterfall.System.Core.WaterfallVFS;
using Waterfall.System.Security;
using Waterfall.System.Security.Runtime;

namespace Waterfall.System.Core.Waterbash.Commands
{
	public class fs : WSHCommand
	{
        public override string HelpNote { get; set; } = "Manages file system";
        public fs()
		{
			paramActions = new Dictionary<string, Action<Watershell>>
			{
				{ "-r", ParamR },
				{ "--register", ParamR },
				{ "--help", ParamHelp },
			};
		}
		public override void Execute(string[] Params, Watershell myBash)
		{
			if (Params.Length == 0)
			{
				ParamR(myBash);
			}
			else
			{
				foreach (var item in Params)
				{
					if (paramActions.ContainsKey(item))
						paramActions[item](myBash);
					else
					{
						myBash.CWrite($"Unknown parameter: {item}");
					}
				}
			}
		}
		public void ParamR(Watershell myBash)
		{
			myBash.CWriteLine("Registering VFS...");
			Kernel.fs = new Cosmos.System.FileSystem.CosmosVFS();
			try
			{
				Cosmos.System.FileSystem.VFS.VFSManager.RegisterVFS(Kernel.fs);
			}
			catch (Exception ex)
			{
				myBash.CWriteLine(ex.Message);
			}
            GlobalConfig.VFSInitialized = true;
			myBash.CWriteLine("Initializing AHCI");
			AHCI_DISK ahci_load = new();
			//ahci_load.Init();
			int allFound = 0;
			for (int i = 0; i < SATA.Devices.Count; i++)
			{
				var MainStorageBlockDevice = SATA.Devices[i];
				Disk disk = new(MainStorageBlockDevice);
				bool found = false;
				for (int j = 0; j < Kernel.fs.Disks.Count; j++)
				{
					if (Kernel.fs.Disks[j].Host == disk.Host)
					{
						found = true;
					}
				}
				if (!found)
				{
					Kernel.fs.Disks.Add(disk);
					Disks.Info.Add(new DiskInfo { Disk = Kernel.fs.Disks[Kernel.fs.Disks.Count - 1], DiskType = DiskType.SATA });
					allFound++;
				}
				else
				{
					Disks.Info.Add(new DiskInfo { Disk = Kernel.fs.Disks[Kernel.fs.Disks.Count - 1], DiskType = DiskType.IDE });
				}
			}
			myBash.CWriteLine("Found " + allFound + " AHCI devices.");
			myBash.CWriteLine("Done.");
		}
		public void ParamHelp(Watershell myBash)
		{
			myBash.CWriteLine("Usage: fs [OPTION]...");
			myBash.CWriteLine("  -r, --register        registers VFS");
			myBash.CWriteLine("      --help            display this help");
		}
	}
}
