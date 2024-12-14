using Cosmos.System.FileSystem;
using System.Collections.Generic;

namespace Waterfall.System.Core.WaterfallVFS
{
	public static class Disks
	{
		public static List<DiskInfo> Info = new List<DiskInfo>();
	}
	public class DiskInfo
	{
		public Disk Disk { get; set; }
		public DiskType DiskType { get; set; }
	}
	public enum DiskType { IDE, SATA }
}
