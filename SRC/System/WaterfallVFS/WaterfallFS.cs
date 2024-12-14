using COROS;
using Cosmos.HAL.BlockDevice;
using Cosmos.HAL.BlockDevice.Ports;
using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterfall.System.WaterfallVFS
{
    public class WaterfallFS
    {
        public List<WDisk> Disks;
        public WaterfallFS()
        {
            Disks = new List<WDisk>();
            AHCI_DISK ahci_load = new();
            ahci_load.Init();
            for (int i = 0; i < SATA.Devices.Count; i++)
            {
                var MainStorageBlockDevice = SATA.Devices[i];
                WDisk disk = new(MainStorageBlockDevice);
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
                    Disks.Add(disk);
                }
            }
        }

    }
}
