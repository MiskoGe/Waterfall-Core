using Waterfall.System.Core.CLI;

namespace Waterfall.System.Drivers.USB
{
	public static class Hub
	{
		public static void Find(CLILogs logs)
		{
			logs.WriteInfo("Scanning for PCI devices...");
			foreach (Cosmos.HAL.PCIDevice device in Cosmos.HAL.PCI.Devices)
			{
				switch (device.ClassCode)
				{
					case 12:
						{
							if (device.Subclass == 3)
							{
								logs.WriteOk($"Found EHCI! ID: {device.DeviceID}! Base addres: {device.BaseAddressBar}, VendorID: {device.VendorID} Class code: {device.ClassCode}, Subclass code: {device.Subclass}");
							}
						}
						break;
				}
				//	logs.WriteInfo($"Found device ID: {device.DeviceID}! Base addres: {device.BaseAddressBar}, VendorID: {device.VendorID} Class code: {device.ClassCode}, Subclass code: {device.Subclass}");
			}
		}
	}
}
