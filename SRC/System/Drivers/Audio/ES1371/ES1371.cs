using Cosmos.Core;
using Cosmos.HAL;
using MOOS.Misc;
using System;
using System.Runtime.InteropServices;
using Waterfall.System.Boot;

namespace MOOS
{
	//TO-DO 44.1khz
	public static unsafe class ES1371
	{
		public static int BAR0;
		public static byte* Buffer;
		private const int CacheSize = 0xFFFFF;

		public static void Initialize()
		{
			var dev = PCI.GetDevice(VendorID.VMWare, DeviceID.VBVGA);
			if (!dev.DeviceExists) return;
			dev.WriteRegister32(0x04, 0x04 | 0x02 | 0x01);
			BAR0 = (int)(dev.BAR0 & ~0x3);
			Buffer = (byte*)NativeMemory.Alloc(CacheSize);
			Console.WriteLine($"[ES1371] BAR0:{BAR0}");

			IOPort.Write32(BAR0 + 0x14, 0x00020000);
			WaterfallBoot.CLILogs.WriteInfo("1");
			IOPort.Write32(BAR0 + 0x14, 0x00180000);
			WaterfallBoot.CLILogs.WriteInfo("2");
			IOPort.Write32(BAR0 + 0x10, 0xeb403800);
			WaterfallBoot.CLILogs.WriteInfo("3");
			SetPlayback2SampleRate(44100);
			WaterfallBoot.CLILogs.WriteInfo("4");
			IOPort.Write32(BAR0 + 0x0c, 0x0c);
			WaterfallBoot.CLILogs.WriteInfo("5");
			IOPort.Write32(BAR0 + 0x38, (uint)Buffer);
			WaterfallBoot.CLILogs.WriteInfo("6");
			IOPort.Write32(BAR0 + 0x3c, 4096/*Audio.SizePerPacket / 47*/);
			WaterfallBoot.CLILogs.WriteInfo("7");
			IOPort.Write32(BAR0 + 0x28, 0x7FFF);
			WaterfallBoot.CLILogs.WriteInfo("8");
			IOPort.Write32(BAR0 + 0x20, 0x0020020C);
			WaterfallBoot.CLILogs.WriteInfo("9");
			IOPort.Write32(BAR0 + 0x00, 0x00000020);
			WaterfallBoot.CLILogs.WriteInfo("10");

			INTs.SetIntHandler(0x20, OnInterrupt);
			WaterfallBoot.CLILogs.WriteInfo("11");
			Audio.HasAudioDevice = true;
			WaterfallBoot.CLILogs.WriteInfo("12");
		}
		static void SetPlayback2SampleRate(int rate)
		{
			long frequency = (rate << 16) / 3000;

			IOPort.Write32(BAR0 + 0x75, (uint)((frequency >> 6) & 0xfc00));
			IOPort.Write32(BAR0 + 0x77, (uint)(frequency >> 1));
		}

		public static void OnInterrupt(ref INTs.IRQContext context)
		{
			WaterfallBoot.CLILogs.WriteInfo("OnInterrupt called");
			uint sts = IOPort.Read32(BAR0 + 0x04);
			if (BitHelpers.IsBitSet(sts, 1))
			{
				IOPort.Write32(BAR0 + 0x20, IOPort.Read32(BAR0 + 0x20) & 0xFFFFFDFF);

				Native.Stosb(Buffer, 0, CacheSize);
				Audio.require(Buffer);
			}
		}
	}
}