using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterfall.System.WaterfallVFS
{
    public class WManagedPartition
    {
        public readonly WPartition Host;
        /// <summary>
        /// The root path of the file system. Example: 0:\
        /// </summary>
        public string RootPath = "";
        /// <summary>
        /// The FileSystem object. Null if not mounted.
        /// </summary>
    //    public FileSystem MountedFS;
        /// <summary>
        /// Does the partition have a known file system?
        /// </summary>
    //    public bool HasFileSystem => MountedFS != null;

        public string LimitFS = null;

        public WManagedPartition(WPartition host, string limitFS = null)
        {
            Host = host;
            LimitFS = limitFS;
        }

        /// <summary>
        /// Zeros out the entire partition
        /// </summary>
        public void Clear()
        {
            for (ulong i = 0; i < Host.BlockCount; i++)
            {
                byte[] data = new byte[512];
                Host.WriteBlock(i, 1, ref data);
            }
         //   MountedFS = null;
            RootPath = "";
        }
    }
}
