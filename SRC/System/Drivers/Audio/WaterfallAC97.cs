using Cosmos.HAL.Audio;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System.Audio;
using Cosmos.System.Audio.IO;
using System;
using Waterfall.System.Boot;

namespace Waterfall.System.Drivers.Audio
{
	public class WaterfallAC97
	{
		public void Initialize()
		{
			try
			{
				var mixer = new AudioMixer();
				//var memAudioStream = new MemoryAudioStream(new SampleFormat(AudioBitDepth.Bits16, 2, true), 44100, Resources.audio);

				var driver = AC97.Initialize(bufferSize: 4096);
				//mixer.Streams.Add(memAudioStream);
				var audioManager = new AudioManager()
				{
					Stream = mixer,
					Output = driver
				};
				audioManager.Enable();
			}
			catch (Exception ex)
			{
				WaterfallBoot.CLILogs.WriteError("[AC97] " + ex.ToString());
			}
		}
	}
}
